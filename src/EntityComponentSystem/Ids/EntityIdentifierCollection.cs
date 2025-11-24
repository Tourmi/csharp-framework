using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.Framework.Collections;

namespace Tourmi.EntityComponentSystem;

internal partial class EntityIdentifierCollection
{
    /// <param name="Archetype"> Archetype of the entity. </param>
    /// <param name="IdentifierIndex"> Index into the <see cref="_entityIdentifiers"/> array. </param>
    /// <param name="ArchetypeIndex"> Index of the entity within the archetype </param>
    private record struct IdentifierToArchetype(Archetype Archetype, uint IdentifierIndex, int ArchetypeIndex);

    private readonly RandomAccessPagedArray<Identifier> _entityIdentifiers = new();
    private readonly RandomAccessPagedArray<IdentifierToArchetype> _entities = new();
    private readonly Dictionary<IdentifierRegion, int> _reservedRegionsToCurrentDataIndex = [];
    private readonly Archetype _emptyArchetype;

    private int _currentDefaultRegionIndex;
    private int[] _defaultRegionDataIndexes; // indexes for the default regions into the _regionsData array
    private IdRegionData[] _regionsData;

    public EntityIdentifierCollection(Archetype emptyArchetype, uint initialCapacity = 0x1000)
    {
        _emptyArchetype = emptyArchetype.ThrowIfNull();
        _entityIdentifiers.EnsureCapacity(initialCapacity);
        _entities.EnsureCapacity(initialCapacity);

        var unreservedRegionData = new IdRegionData() { Offset = 1, Capacity = uint.MaxValue - 1, };
        _regionsData = [unreservedRegionData];
        _defaultRegionDataIndexes = [0];
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
    /// Creates a new entity, and returns its identifier.
    /// </summary>
    public Identifier Create()
    {
        if (_currentDefaultRegionIndex < 0)
        {
            throw new InvalidOperationException("Ran out of entity Ids to create a new Entity with.");
        }

        var regionDataIndex = _defaultRegionDataIndexes[_currentDefaultRegionIndex];
        ref var regionData = ref _regionsData[regionDataIndex];
        var identifier = Create(ref regionData);
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

    /// <summary>
    /// Creates a new entity in the given <paramref name="idRegion"/>, and returns its identifier.
    /// </summary>
    public Identifier Create(IdentifierRegion? idRegion)
    {
        if (idRegion is null)
        {
            return Create();
        }

        var regionDataIndex = _reservedRegionsToCurrentDataIndex[idRegion];
        ref var regionData = ref _regionsData[regionDataIndex];
        if (regionData.IsFull)
        {
            throw new InvalidOperationException($"Ran out of entity Ids in the region {idRegion}");
        }

        return Create(ref regionData);
    }

    public void Kill(Identifier entityId)
    {
        if (!IsAlive(entityId))
        {
            return;
        }

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
    /// Reserves the given region for future entity generation.
    /// </summary>
    public void Reserve(IdentifierRegion region)
    {
        if (_reservedRegionsToCurrentDataIndex.ContainsKey(region.ThrowIfNull()))
        {
            // Already reserved, just return.
            return;
        }

        if (_reservedRegionsToCurrentDataIndex.Keys.Any(r => r.OverlapsWith(region)))
        {
            throw new InvalidOperationException($"Cannot reserve identifier region {region}, as it overlaps with already reserved regions.");
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

    public void AddComponent(Identifier entityId, Identifier componentId, Type? componentDataType)
    {
        ref var entity = ref _entities[entityId.ShortId];
        if (entity.Archetype.HasComponent(componentId))
        {
            return;
        }

        var (updatedEntity, toUpdate) = entity.Archetype.AddComponent(entity.ArchetypeIndex, componentId, componentDataType);
        if (toUpdate.HasValue)
        {
            var (entityToUpdate, entityNewIndex) = toUpdate.Get();
            ref var toUpdateIdToArchetype = ref _entities[entityToUpdate.ShortId];
            toUpdateIdToArchetype.ArchetypeIndex = entityNewIndex;
        }

        entity.Archetype = updatedEntity.NewArchetype;
        entity.ArchetypeIndex = updatedEntity.NewIndex;
    }

    public void RemoveComponent(Identifier entityId, Identifier componentId)
    {
        ref var entity = ref _entities[entityId.ShortId];
        if (!entity.Archetype.HasComponent(componentId))
        {
            return;
        }

        var (updatedEntity, toUpdate) = entity.Archetype.RemoveComponent(entity.ArchetypeIndex, componentId);
        if (toUpdate.HasValue)
        {
            var (entityToUpdate, entityNewIndex) = toUpdate.Get();
            ref var toUpdateIdToArchetype = ref _entities[entityToUpdate.ShortId];
            toUpdateIdToArchetype.ArchetypeIndex = entityNewIndex;
        }

        entity.Archetype = updatedEntity.NewArchetype;
        entity.ArchetypeIndex = updatedEntity.NewIndex;
    }

    public bool HasComponent(Identifier entityId, Identifier componentId)
    {
        ref var entity = ref _entities[entityId.ShortId];
        return entity.Archetype.HasComponent(componentId);
    }

    public T? GetComponent<T>(Identifier entityId, Identifier componentId)
    {
        ref var entity = ref _entities[entityId.ShortId];
        if (!entity.Archetype.HasComponent(componentId))
        {
            return default;
        }

        return entity.Archetype.GetValue<T>(entity.ArchetypeIndex, componentId);
    }

    public void SetComponent<T>(Identifier entityId, Identifier componentId, T value)
    {
        ref var entity = ref _entities[entityId.ShortId];
        entity.Archetype.SetValue(entity.ArchetypeIndex, componentId, value);
    }

    private Identifier Create(ref IdRegionData regionData)
    {
        // todo: allow passing in the type of identifier to use
        var type = IdentifierTypes.None;
        var typeBits = (uint)type << Identifier.TypesBitOffset;

        if (regionData.AliveCount < regionData.InitializedCount)
        {
            // Reuse dead identifiers
            ref var id = ref _entityIdentifiers[regionData.NextAliveIndex];
            id = (id & ~Identifier.TypesBitMask) | typeBits;

            var index = _emptyArchetype.AddEntity(id);
            _entities[id.ShortId] = new(_emptyArchetype, regionData.NextAliveIndex, index);

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

            var index = _emptyArchetype.AddEntity(id);
            _entities[id.ShortId] = new(_emptyArchetype, id.ShortId, index);

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
