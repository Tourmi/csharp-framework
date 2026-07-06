namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Collection that contains a world's entitity IDs.
/// </summary>
internal partial class IdentifierCollection
{
    /// <summary>
    /// Stores all identifiers.
    /// </summary>
    private readonly LazyPagedArray<Identifier> _identifiers = new();

    /// <summary>
    /// When given an <see cref="Identifier"/>'s <see cref="Identifier.ShortId"/>, 
    /// returns the index that the Id is stored in <see cref="_identifiers"/>.
    /// </summary>
    private readonly LazyPagedArray<uint> _idToIndex = new();
    private readonly HashSet<IdentifierRegion> _reservedRegions = [];

    /// <summary>
    /// index into the _regionsData array
    /// </summary>
    private int _currentRegionIndex;
    private IdRegionData[] _regionsData;

    public IdentifierCollection(uint initialCapacity = 0x1000)
    {
        _identifiers.EnsureCapacity(initialCapacity);
        _idToIndex.EnsureCapacity(initialCapacity);

        var unreservedRegionData = new IdRegionData() { Offset = 1, Capacity = uint.MaxValue - 1, };
        _regionsData = [unreservedRegionData];
    }

    /// <summary>
    /// Reserves the given region for future entity generation.
    /// </summary>
    public void Reserve(IdentifierRegion region)
    {
        if (region.ThrowIfNull().Amount is 0)
        {
            // Nothing to reserve, just return.
            return;
        }

        if (region.Offset != 0 && _reservedRegions.Any(r => r != region && r.OverlapsWith(region)))
        {
            var overlappingRegions = string.Join("\n\t", _reservedRegions.Where(r => r.OverlapsWith(region)).Select(r => r.ToString()));
            throw new InvalidOperationException(
                $"Cannot reserve identifier region {region}, " +
                $"as it overlaps with the following reserved regions:\n\t{overlappingRegions}");
        }

        if (!_reservedRegions.Add(region))
        {
            // Already reserved, just return.
            return;
        }

        if (region.Offset is 0)
        {
            for (var i = _regionsData.Length - 1; i >= 0; i--)
            {
                var regionData = _regionsData[i];
                if (regionData.Capacity >= region.Amount)
                {
                    region.Offset = regionData.EndIdInclusive - region.Amount + 1;
                    break;
                }
            }
        }

        if (region.Offset is 0)
        {
            throw new InvalidOperationException($"Failed to reserve id region '{region}', no available room.");
        }

        var reservedRegionData = new IdRegionData() { Offset = region.Offset, Capacity = region.Amount };

        _regionsData = [.. _regionsData.SelectMany(r => r.SplitWith(reservedRegionData)).OrderBy(r => r.Offset)];
        _currentRegionIndex = _regionsData
            .Index()
            .Where(ri => !ri.Item.IsFull)
            .Select(ri => ri.Index)
            .FirstOrDefault(-1);
    }

    /// <summary>
    /// Returns <see langword="true"/> if the <paramref name="region"/> is reserved, <see langword="false"/> otherwise.
    /// </summary>
    public bool IsReserved(IdentifierRegion region) => _reservedRegions.Contains(region);

    /// <summary>
    /// Creates a new id and returns it.
    /// </summary>
    public Identifier Create(IdentifierTypes entityTypes = IdentifierTypes.None)
    {
        if (_currentRegionIndex < 0)
        {
            throw new InvalidOperationException("Ran out of entity Ids to create a new Entity with.");
        }

        ref var regionData = ref _regionsData[_currentRegionIndex];
        var identifier = Create(ref regionData, entityTypes);

        if (regionData.IsFull)
        {
            _currentRegionIndex = -1;
            for (var i = 0; i < _regionsData.Length; i++)
            {
                if (_regionsData[i].IsFull)
                {
                    continue;
                }

                _currentRegionIndex = i;
                break;
            }
        }

        return identifier;

        Identifier Create(ref IdRegionData regionData, IdentifierTypes types)
        {
            var typeBits = (uint)types << Identifier.TypesBitOffset;

            if (regionData.AliveCount < regionData.InitializedCount)
            {
                // Reuse dead identifiers
                ref var id = ref _identifiers[regionData.NextAliveIndex];
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
                _identifiers.Insert(id.ShortId, id);
                _idToIndex[id.ShortId] = id.ShortId;

                checked
                {
                    regionData.AliveCount++;
                    regionData.InitializedCount++;
                }

                return id;
            }
        }
    }

    /// <summary>
    /// Returns true if the given id is currently in use.
    /// </summary>
    public bool IsInUse(Identifier entityId)
    {
        if (entityId.ShortId is 0)
        {
            return false;
        }

        return _identifiers[_idToIndex[entityId.ShortId]] == entityId;
    }

    /// <summary>
    /// Releases the given <paramref name="entityId"/> to the pool.
    /// </summary>
    /// <remarks>
    /// Purposefully does not validate if the <paramref name="entityId"/> is in use for better performance.
    /// </remarks>
    public void Free(Identifier entityId)
    {
        ref var region = ref GetIdRegionData(entityId);
        var oldIndex = _idToIndex[entityId.ShortId];
        var oldIdNewVersion = entityId.IncrementVersion();

        // Swap the last alive entity with the one we just freed.
        var lastAliveIndex = region.NextAliveIndex - 1;
        var lastAliveId = _identifiers[lastAliveIndex];
        _identifiers[oldIndex] = lastAliveId;
        _identifiers[lastAliveIndex] = oldIdNewVersion;
        _idToIndex[lastAliveId.ShortId] = oldIndex;
        _idToIndex[entityId.ShortId] = default;

        checked
        {
            region.AliveCount--;
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
