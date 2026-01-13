using Tourmi.Framework.Collections;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Collection that contains a world's entitity IDs.
/// </summary>
internal partial class IdentifierCollection
{
    private readonly LazyPagedArray<Identifier> _indexToId = new();
    private readonly LazyPagedArray<uint> _idToIndex = new();
    private readonly Dictionary<IdentifierRegion, int> _reservedRegionsToCurrentDataIndex = [];

    private int _currentDefaultRegionIndex; // index into the _defaultRegionDataIndexes array
    private int[] _defaultRegionDataIndexes; // indexes for the default regions into the _regionsData array
    private IdRegionData[] _regionsData;

    public IdentifierCollection(uint initialCapacity = 0x1000)
    {
        _indexToId.EnsureCapacity(initialCapacity);
        _idToIndex.EnsureCapacity(initialCapacity);

        var unreservedRegionData = new IdRegionData() { Offset = 1, Capacity = uint.MaxValue - 1, };
        _regionsData = [unreservedRegionData];
        _defaultRegionDataIndexes = [0];
    }

    /// <summary>
    /// Reserves the given region for future entity generation.
    /// </summary>
    public void Reserve(IdentifierRegion region)
    {
        if (region?.Amount is null or 0)
        {
            // Nothing to reserve, just return.
            return;
        }

        if (_reservedRegionsToCurrentDataIndex.ContainsKey(region.ThrowIfNull()))
        {
            // Already reserved, just return.
            return;
        }

        if (region.Offset is 0)
        {
            for (var i = 0; i < _defaultRegionDataIndexes.Length; i++)
            {
                var regionData = _regionsData[_defaultRegionDataIndexes[i]];
                if (regionData.Capacity >= region.Amount)
                {
                    region.Offset = regionData.Offset;
                    break;
                }
            }
        }

        if (region.Offset is 0)
        {
            throw new InvalidOperationException($"Failed to reserve id region: {region}");
        }

        if (_reservedRegionsToCurrentDataIndex.Keys.Any(r => r.OverlapsWith(region)))
        {
            var overlappingRegions = string.Join("\n\t", _reservedRegionsToCurrentDataIndex.Keys.Where(r => r.OverlapsWith(region)).Select(r => r.ToString()));
            throw new InvalidOperationException(
                $"Cannot reserve identifier region {region}, " +
                $"as it overlaps with the following reserved regions:\n\t{overlappingRegions}");
        }

        var newRegionData = new IdRegionData() { Offset = region.Offset, Capacity = region.Amount };

        var existingRegions = _regionsData
            .SelectMany((r, i) =>
            {
                if (!_defaultRegionDataIndexes.Contains(i))
                {
                    return [(Region: r, IsDefault: false)];
                }

                return r.SplitWith(newRegionData).Select(r => (Region: r, IsDefault: true));
            })
            .Append((Region: newRegionData, IsDefault: false))
            .OrderBy(ri => ri.Region.Offset)
            .ToArray();

        _regionsData = [.. existingRegions.Select(ri => ri.Region)];
        _defaultRegionDataIndexes = [.. existingRegions
            .Select((ri, i) => (ri.Region, ri.IsDefault, Index: i))
            .Where(rii => rii.IsDefault)
            .Select(rii => rii.Index)];
        _currentDefaultRegionIndex = _defaultRegionDataIndexes
            .Select((r, i) => (RegionIndex: r, ArrayIndex: i))
            .Where(ri => !_regionsData[ri.RegionIndex].IsFull)
            .Select(ri => ri.ArrayIndex)
            .DefaultIfEmpty(-1)
            .First();

        _reservedRegionsToCurrentDataIndex.Add(region, -1);

        foreach (var oldRegion in _reservedRegionsToCurrentDataIndex.Keys.ToArray())
        {
            _reservedRegionsToCurrentDataIndex[oldRegion] = _regionsData
                .Select((r, i) => (Region: r, Index: i))
                .First(ri => ri.Region.Offset == oldRegion.Offset)
                .Index;
        }
    }

    /// <summary>
    /// Creates a new id in the given <paramref name="idRegion"/>, (or the default id space when <see langword="null"/>), and returns its identifier.
    /// </summary>
    public Identifier Create(IdentifierTypes entityTypes = IdentifierTypes.None, IdentifierRegion? idRegion = null)
    {
        if (idRegion is null)
        {
            return CreateDefault(entityTypes);
        }

        var regionDataIndex = _reservedRegionsToCurrentDataIndex[idRegion];
        ref var regionData = ref _regionsData[regionDataIndex];
        if (regionData.IsFull)
        {
            throw new InvalidOperationException($"Ran out of entity Ids in the region {idRegion}");
        }

        return Create(ref regionData, entityTypes);

        // Creates an entity in the unreserved id space.
        Identifier CreateDefault(IdentifierTypes entityTypes)
        {
            if (_currentDefaultRegionIndex < 0)
            {
                throw new InvalidOperationException("Ran out of entity Ids to create a new Entity with.");
            }

            var regionDataIndex = _defaultRegionDataIndexes[_currentDefaultRegionIndex];
            ref var regionData = ref _regionsData[regionDataIndex];
            var identifier = Create(ref regionData, entityTypes);
            if (regionData.IsFull)
            {
                _currentDefaultRegionIndex = -1;
                for (var i = 0; i < _defaultRegionDataIndexes.Length; i++)
                {
                    if (_regionsData[_defaultRegionDataIndexes[i]].IsFull)
                    {
                        continue;
                    }

                    _currentDefaultRegionIndex = i;
                    break;
                }
            }

            return identifier;
        }
    }

    /// <summary>
    /// Returns true if the given id is in use (usually when an entity is created).
    /// </summary>
    public bool IsUsed(Identifier entityId)
    {
        if (entityId.ShortId is 0)
        {
            return false;
        }

        return _indexToId[_idToIndex[entityId.ShortId]] == entityId;
    }

    /// <summary>
    /// Releases the given <paramref name="entityId"/> to the pool.
    /// </summary>
    public void Free(Identifier entityId)
    {
        ref var region = ref GetIdRegionData(entityId);
        var oldIndex = _idToIndex[entityId.ShortId];

        _indexToId[oldIndex] = entityId.IncrementVersion();
        _idToIndex[entityId.ShortId] = default;

        checked
        {
            region.AliveCount--;
        }

        var lastAliveIndex = region.NextAliveIndex;
        var lastAliveId = _indexToId[lastAliveIndex];
        _indexToId[oldIndex] = _indexToId[lastAliveIndex];
        _idToIndex[lastAliveId.ShortId] = oldIndex;
    }

    private Identifier Create(ref IdRegionData regionData, IdentifierTypes types)
    {
        var typeBits = (uint)types << Identifier.TypesBitOffset;

        if (regionData.AliveCount < regionData.InitializedCount)
        {
            // Reuse dead identifiers
            ref var id = ref _indexToId[regionData.NextAliveIndex];
            id = (id & ~Identifier.TypesBitMask) | typeBits;

            _idToIndex[id.ShortId] = regionData.NextAliveIndex;

            checked
            {
                regionData.AliveCount++;
            }

            return id;
        }
        else
        {
            // New identifier needed
            var id = new Identifier(regionData.NextInitializedIndex);
            id |= typeBits;
            _indexToId.Insert(id.ShortId, id);
            _idToIndex[id.ShortId] = id.ShortId;

            checked
            {
                regionData.AliveCount++;
                regionData.InitializedCount++;
            }

            return id;
        }
    }

    private ref IdRegionData GetIdRegionData(Identifier entityId)
    {
        for (var i = 0; i < _regionsData.Length; i++)
        {
            ref var regionData = ref _regionsData[i];
            if (regionData.Contains(entityId.ShortId))
            {
                return ref regionData;
            }
        }

        throw new InvalidOperationException("Identifier is not contained in any of the existing regions.");
    }
}
