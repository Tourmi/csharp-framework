using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.Framework.Collections;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Collection that contains a world's entitity IDs, as well as links to the archetypes of an entity.
/// </summary>
/// <remarks>
/// No validation is done at any point for any component operations within this class.
/// Make sure to always pass-in proper information
/// </remarks>
internal partial class IdentifierCollection
{
    /// <param name="Archetype"> Archetype of the entity. </param>
    /// <param name="IdentifierIndex"> Index into the <see cref="_entityIdentifiers"/> array. </param>
    /// <param name="ArchetypeIndex"> Index of the entity within the archetype </param>
    private record struct IdentifierToArchetype(Archetype Archetype, uint IdentifierIndex, int ArchetypeIndex);

    private readonly RandomAccessPagedArray<Identifier> _entityIdentifiers = new();
    private readonly RandomAccessPagedArray<IdentifierToArchetype> _entities = new();
    private readonly Dictionary<IdentifierRegion, int> _reservedRegionsToCurrentDataIndex = [];
    private readonly Archetype _emptyArchetype;

    private int _currentDefaultRegionIndex; // index into the _defaultRegionDataIndexes array
    private int[] _defaultRegionDataIndexes; // indexes for the default regions into the _regionsData array
    private IdRegionData[] _regionsData;

    /// <summary>
    /// Overrides the default region to use when creating new entities. 
    /// If <see langword="null"/>, then the unreserved id space with be used.
    /// </summary>
    public IdentifierRegion? DefaultRegionOverride
    {
        get;
        set
        {
            if (value != null && !_reservedRegionsToCurrentDataIndex.ContainsKey(value))
            {
                throw new ArgumentException("Cannot set region override to an un-reserved region", nameof(value));
            }

            field = value;
        }
    }

    public IdentifierCollection(Archetype emptyArchetype, uint initialCapacity = 0x1000)
    {
        _emptyArchetype = emptyArchetype.ThrowIfNull();
        _entityIdentifiers.EnsureCapacity(initialCapacity);
        _entities.EnsureCapacity(initialCapacity);

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
    /// Creates a new entity in the given <paramref name="idRegion"/>, (or the default id space when <see langword="null"/>), and returns its identifier.
    /// </summary>
    public Identifier Create(IdentifierTypes entityTypes = IdentifierTypes.None, IdentifierRegion? idRegion = null)
    {
        idRegion ??= DefaultRegionOverride;
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
    /// Returns true if the given entity identifier points to an entity that is alive.
    /// </summary>
    public bool IsAlive(Identifier entityId)
    {
        if (entityId.ShortId is 0)
        {
            return false;
        }

        var entity = _entities[entityId.ShortId];
        if (entity.IdentifierIndex is 0)
        {
            return false;
        }

        return _entityIdentifiers[entity.IdentifierIndex] == entityId;
    }

    /// <summary>
    /// Releases the given <paramref name="entityId"/> to the pool.
    /// </summary>
    public void Free(Identifier entityId)
    {
        ref var oldEntity = ref _entities[entityId.ShortId];
        var oldIndex = oldEntity.IdentifierIndex;
        ref var oldId = ref _entityIdentifiers[oldIndex];
        ref var region = ref GetIdRegionData(entityId);

        oldId = oldId.IncrementVersion();
        var toUpdate = oldEntity.Archetype.RemoveEntity(oldEntity.ArchetypeIndex);
        oldEntity = default;

        if (toUpdate.HasValue)
        {
            var (entityToUpdate, entityNewIndex) = toUpdate.Get();
            ref var toUpdateIdToArchetype = ref _entities[entityToUpdate.ShortId];
            toUpdateIdToArchetype.ArchetypeIndex = entityNewIndex;
        }

        checked
        {
            region.AliveCount--;
        }

        var lastAliveIndex = region.NextAliveIndex;

        if (lastAliveIndex == oldIndex)
        {
            // The entity being killed was the last alive entity for the id region, we don't need to swap.
            return;
        }

        ref var lastAliveId = ref _entityIdentifiers[lastAliveIndex];
        ref var lastAliveEntity = ref _entities[lastAliveId.ShortId];

        (lastAliveId, oldId) = (oldId, lastAliveId);
        lastAliveEntity.IdentifierIndex = oldIndex;
    }

    /// <summary>
    /// Adds the component to the entity.
    /// Note that this does not check if the entity already has the component.
    /// </summary>
    public void AddComponent(Identifier entityId, Identifier componentId, Type? dataType)
    {
        ref var entity = ref _entities[entityId.ShortId];
        var (updatedEntity, toUpdate) = entity.Archetype.AddComponent(entity.ArchetypeIndex, componentId, dataType);
        if (toUpdate.HasValue)
        {
            var (entityToUpdate, entityNewIndex) = toUpdate.Get();
            ref var toUpdateIdToArchetype = ref _entities[entityToUpdate.ShortId];
            toUpdateIdToArchetype.ArchetypeIndex = entityNewIndex;
        }

        entity.Archetype = updatedEntity.Archetype;
        entity.ArchetypeIndex = updatedEntity.Index;
    }

    /// <summary>
    /// Adds the component to the entity.
    /// Note that this does not check if the entity has the component.
    /// </summary>
    public void RemoveComponent(Identifier entityId, Identifier componentId)
    {
        ref var entity = ref _entities[entityId.ShortId];
        var (updatedEntity, toUpdate) = entity.Archetype.RemoveComponent(entity.ArchetypeIndex, componentId);
        if (toUpdate.HasValue)
        {
            var (entityToUpdate, entityNewIndex) = toUpdate.Get();
            ref var toUpdateIdToArchetype = ref _entities[entityToUpdate.ShortId];
            toUpdateIdToArchetype.ArchetypeIndex = entityNewIndex;
        }

        entity.Archetype = updatedEntity.Archetype;
        entity.ArchetypeIndex = updatedEntity.Index;
    }

    /// <summary>
    /// Checks whether or not the entity has the given component
    /// </summary>
    public bool HasComponent(Identifier entityId, Identifier componentId)
    {
        var entity = _entities[entityId.ShortId];
        return entity.Archetype.HasComponent(componentId);
    }

    public T? GetComponent<T>(Identifier entityId, Identifier componentId)
    {
        var entity = _entities[entityId.ShortId];
        return entity.Archetype.GetValue<T>(entity.ArchetypeIndex, componentId);
    }

    public ref T? GetRefComponent<T>(Identifier entityId, Identifier componentId)
    {
        var entity = _entities[entityId.ShortId];
        return ref entity.Archetype.GetValueRef<T>(entity.ArchetypeIndex, componentId);
    }

    public void SetComponent<T>(Identifier entityId, Identifier componentId, T value)
    {
        var entity = _entities[entityId.ShortId];
        entity.Archetype.SetValue(entity.ArchetypeIndex, componentId, value);
    }

    /// <summary>
    /// Used for debugging purposes
    /// </summary>
    internal Archetype? GetArchetype(Identifier entityId)
    {
        if (!IsAlive(entityId))
        {
            return null;
        }

        return _entities[entityId.ShortId].Archetype;
    }

    /// <summary>
    /// Used for debugging purposes
    /// </summary>
    internal ArchetypeEntityEntry? GetArchetypeEntry(Identifier entityId)
    {
        if (!IsAlive(entityId))
        {
            return null;
        }

        var entity = _entities[entityId.ShortId];
        return new(entity.Archetype, entity.ArchetypeIndex);
    }

    private Identifier Create(ref IdRegionData regionData, IdentifierTypes types)
    {
        var typeBits = (uint)types << Identifier.TypesBitOffset;

        if (regionData.AliveCount < regionData.InitializedCount)
        {
            // Reuse dead identifiers
            ref var id = ref _entityIdentifiers[regionData.NextAliveIndex];
            id = (id & ~Identifier.TypesBitMask) | typeBits;

            var entry = _emptyArchetype.AddEntity(id);
            _entities[id.ShortId] = new(_emptyArchetype, regionData.NextAliveIndex, entry.Index);

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
            _entityIdentifiers.Insert(id.ShortId, id);

            var entry = _emptyArchetype.AddEntity(id);
            _entities[id.ShortId] = new(_emptyArchetype, id.ShortId, entry.Index);

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
