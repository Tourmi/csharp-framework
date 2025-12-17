using Tourmi.Framework.Collections;
using MovedEntity = (Tourmi.EntityComponentSystem.Ids.Identifier Id, int NewIndex);

namespace Tourmi.EntityComponentSystem.Archetypes;

/// <summary>
/// Contains all of the entities and their archetypes, and allows for operations on entities.
/// </summary>
/// <remarks>
/// No validation is done at any point for any component operations within this class.
/// Make sure to always pass-in proper information
/// </remarks>
internal class EntityArchetypeCollection
{
    private readonly LazyPagedArray<ArchetypeEntityEntry> _entities = new();
    private readonly IdentifiersToArchetypeDictionary _archetypes = new();

    public EntityArchetypeCollection(uint initialCapacity = 0x1000)
    {
        _entities.EnsureCapacity(initialCapacity);
    }

    /// <summary>
    /// Mapping from components to their archetypes.
    /// </summary>
    internal IdentifiersToArchetypeDictionary ComponentsToArchetypes => _archetypes;

    /// <summary>
    /// Creates a new entity and inserts it into the empty archetype
    /// </summary>
    public void Create(Identifier id) => _entities[id.ShortId] = _archetypes.EmptyArchetype.AddEntity(id);

    /// <summary>
    /// Kills the entity with the given <paramref name="entityId"/>
    /// </summary>
    public void Kill(Identifier entityId)
    {
        var movedEntity = _entities[entityId.ShortId].Remove();
        ProcessMovedEntity(movedEntity);
    }

    /// <summary>
    /// Adds the component to the entity.
    /// Note that this does not check if the entity already has the component.
    /// </summary>
    public void AddComponent(Identifier entityId, Identifier componentId, Type? dataType)
    {
        var movedEntity = _entities[entityId.ShortId].AddComponent(componentId, dataType);
        ProcessMovedEntity(movedEntity);
    }

    /// <summary>
    /// Adds the component to the entity.
    /// Note that this does not check if the entity has the component.
    /// </summary>
    public void RemoveComponent(Identifier entityId, Identifier componentId)
    {
        var movedEntity = _entities[entityId.ShortId].RemoveComponent(componentId);
        ProcessMovedEntity(movedEntity);
    }

    /// <summary>
    /// Checks whether or not the entity has the given component
    /// </summary>
    public bool HasComponent(Identifier entityId, Identifier componentId) 
        => _entities[entityId.ShortId].Archetype.HasComponent(componentId);

    /// <summary>
    /// Returns the value of the component.
    /// </summary>
    public T? GetComponent<T>(Identifier entityId, Identifier componentId) 
        => _entities[entityId.ShortId].GetValue<T>(componentId);

    /// <summary>
    /// Returns a reference to the value of the component.
    /// </summary>
    public ref T? GetRefComponent<T>(Identifier entityId, Identifier componentId) 
        => ref _entities[entityId.ShortId].GetValueRef<T>(componentId);

    /// <summary>
    /// Sets the value of the component.
    /// </summary>
    public void SetComponent<T>(Identifier entityId, Identifier componentId, T value) 
        => _entities[entityId.ShortId].SetValue(componentId, value);

    /// <summary>
    /// Returns all existing archetypes that contain the 
    /// given component represented by <paramref name="componentId"/>
    /// </summary>
    internal IEnumerable<Archetype> GetArchetypesContainingComponent(Identifier componentId)
        => _archetypes[componentId];

    /// <summary>
    /// Used for debugging purposes
    /// </summary>
    internal Archetype? GetEntityArchetype(Identifier entityId) 
        => _entities[entityId.ShortId].Archetype;

    /// <summary>
    /// Used for debugging purposes
    /// </summary>
    internal ArchetypeEntityEntry? GetArchetypeEntry(Identifier entityId)
    {
        var entry = _entities[entityId.ShortId];
        return entry.Archetype is null ? null : entry;
    }

    private void ProcessMovedEntity(Option<MovedEntity> entity)
    {
        if (!entity.HasValue)
        {
            return;
        }

        var (entityToUpdate, entityNewIndex) = entity.Get();
        _entities[entityToUpdate.ShortId].Index = entityNewIndex;
    }
}
