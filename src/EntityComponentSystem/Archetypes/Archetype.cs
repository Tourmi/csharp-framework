using System.Collections.Immutable;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;
using MovedEntity = (Tourmi.EntityComponentSystem.Ids.Identifier Id, int NewIndex);

namespace Tourmi.EntityComponentSystem.Archetypes;

/// <summary>
/// Contains the data of all entities that match the types the archetype is comprised of.
/// </summary>
internal partial class Archetype
{
    private readonly Dictionary<Identifier, Archetype> _parentArchetypes = [];
    private readonly Dictionary<Identifier, Archetype> _childArchetypes = [];
    private readonly List<Identifier> _entities = [];

    private readonly SharedData _sharedData;
    private readonly ImmutableSortedDictionary<Identifier, Type?> _componentsDataType;
    private readonly ImmutableSortedDictionary<Identifier, IComponentCollection> _componentsData;

    private Archetype(SharedData sharedData, IEnumerable<KeyValuePair<Identifier, Type?>> componentTypes)
    {
        _sharedData = sharedData;
        _componentsDataType = componentTypes.ToImmutableSortedDictionary();
        _componentsData = _componentsDataType.ToImmutableSortedDictionary(c => c.Key, c => _sharedData.CreateComponentCollection(c.Value));
    }

    private Archetype()
    {
        _sharedData = new(this);
        _componentsDataType = Enumerable.Empty<KeyValuePair<Identifier, Type?>>().ToImmutableSortedDictionary();
        _componentsData = Enumerable.Empty<KeyValuePair<Identifier, IComponentCollection>>().ToImmutableSortedDictionary();
    }

    /// <summary>
    /// All the components represented by this archetype
    /// </summary>
    public IEnumerable<Identifier> Components => _componentsData.Keys;

    /// <summary>
    /// The amount of entities present in this archetype
    /// </summary>
    public int EntityCount => _entities.Count;

    /// <summary>
    /// Creates a new Empty Archetype.
    /// </summary>
    public static Archetype Create() => new();

    /// <summary>
    /// Returns whether or not the archetype contains the given component
    /// </summary>
    public bool HasComponent(Identifier componentId) => _componentsData.ContainsKey(componentId);

    /// <summary>
    /// Adds the entity to the archetype and returns the index it has in the archetype
    /// </summary>
    public ArchetypeEntityEntry AddEntity(Identifier entityId)
    {
        foreach (var componentCollection in _componentsData.Values)
        {
            componentCollection.AddEntry();
        }

        return AddEntityInternal(entityId);
    }

    /// <summary>
    /// Removes the entity at the given index from the archetype, 
    /// returning the entity that needed to be moved to complete the operation and its new index.
    /// </summary>
    public Option<MovedEntity> RemoveEntity(int index)
    {
        foreach (var componentCollection in _componentsData.Values)
        {
            componentCollection.RemoveEntry(index);
        }

        return RemoveEntityInternal(index);
    }

    /// <summary>
    /// Adds the given component to the entity, moving it to a new archetype in the process.
    /// </summary>
    /// <param name="currentIndex">Current index of the entity in the archetype</param>
    /// <param name="componentIdentifier">Identifier of the component to add</param>
    /// <param name="componentDataType">Datatype of the component</param>
    /// <returns>
    /// The entity's new archetype and index within the archetype, 
    /// as well as an entity that might have needed to move in the current archetype
    /// </returns>
    public (ArchetypeEntityEntry CurrentEntity, Option<MovedEntity> MovedEntity) AddComponent(int currentIndex, Identifier componentIdentifier, Type? componentDataType)
    {
        if (!_parentArchetypes.TryGetValue(componentIdentifier, out var targetArchetype))
        {
            targetArchetype = _sharedData.GetArchetype([.. _componentsDataType, new(componentIdentifier, componentDataType)]);
            _parentArchetypes[componentIdentifier] = targetArchetype;
        }

        foreach (var (collectionId, componentCollection) in _componentsData)
        {
            targetArchetype._componentsData[collectionId].TakeEntryFrom(componentCollection, currentIndex);
        }

        targetArchetype._componentsData[componentIdentifier].AddEntry();

        return MoveEntityToInternal(targetArchetype, currentIndex);
    }

    /// <summary>
    /// Removes the given component from the entity, moving it to a new archetype in the process.
    /// </summary>
    /// <param name="currentIndex">Current index of the entity in the archetype</param>
    /// <param name="componentId">Identifier of the component to add</param>
    /// <returns>
    /// The entity's new archetype and index within the archetype,
    /// as well as an entity that might have needed to move in the current archetype
    /// </returns>
    public (ArchetypeEntityEntry CurrentEntity, Option<MovedEntity> MovedEntity) RemoveComponent(int currentIndex, Identifier componentId)
    {
        if (!_childArchetypes.TryGetValue(componentId, out var targetArchetype))
        {
            targetArchetype = _sharedData.GetArchetype(_componentsDataType.Where(k => k.Key != componentId));
            _childArchetypes[componentId] = targetArchetype;
        }

        foreach (var (collectionId, componentCollection) in _componentsData)
        {
            if (collectionId == componentId)
            {
                continue;
            }

            targetArchetype._componentsData[collectionId].TakeEntryFrom(componentCollection, currentIndex);
        }

        return MoveEntityToInternal(targetArchetype, currentIndex);
    }

    /// <summary>
    /// Sets the value of the component for the entity at the given <paramref name="index"/>
    /// </summary>
    public void SetValue<T>(int index, Identifier componentId, T value)
    {
        if (_componentsData[componentId] is not IContravariantComponentCollection<T> collection)
        {
            throw new InvalidOperationException($"Type {typeof(T)} not supported by component {componentId}");
        }

        collection[index] = value;
    }

    /// <summary>
    /// Gets the value of the component for the entity at the given <paramref name="index"/>
    /// </summary>
    public T? GetValue<T>(int index, Identifier componentId)
    {
        if (_componentsData[componentId] is not ICovariantComponentCollection<T> collection)
        {
            throw new InvalidOperationException($"Type {typeof(T)} not supported by component {componentId}");
        }

        return collection[index];
    }

    /// <summary>
    /// Gets a reference to the value of the component for the entity at the given <paramref name="index"/>
    /// </summary>
    public ref T? GetValueRef<T>(int index, Identifier componentId)
    {
        if (_componentsData[componentId] is not ComponentCollection<T> collection)
        {
            throw new InvalidOperationException($"Type {typeof(T)} not supported by component {componentId}");
        }

        return ref collection[index];
    }

    internal object? GetDebugValue(int index, Identifier componentId) => _componentsData[componentId].GetDebugValue(index);

    /// <summary>
    /// Moves the entity between archetypes without touching component collections
    /// </summary>
    private (ArchetypeEntityEntry CurrentEntity, Option<MovedEntity> MovedEntity) MoveEntityToInternal(Archetype targetArchetype, int currentIndex) =>
        (targetArchetype.AddEntityInternal(_entities[currentIndex]), RemoveEntityInternal(currentIndex));

    /// <summary>
    /// Removes the entity without touching the component collections
    /// </summary>
    private Option<MovedEntity> RemoveEntityInternal(int index)
    {
        var movedEntity = _entities.Pop();
        if (index == _entities.Count)
        {
            return default;
        }

        _entities[index] = movedEntity;

        return (movedEntity, index);
    }

    /// <summary>
    /// Adds the entity to the current archetype without touching the component collections
    /// </summary>
    private ArchetypeEntityEntry AddEntityInternal(Identifier entityId)
    {
        _entities.Add(entityId);
        return new(this, _entities.Count - 1);
    }
}
