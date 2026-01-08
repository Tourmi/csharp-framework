using System.Collections.Immutable;
using System.Runtime.InteropServices;
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

    private readonly ArchetypeSharedData _sharedData;
    private readonly ImmutableSortedDictionary<Identifier, Type?> _componentDataTypes;
    private readonly ImmutableSortedDictionary<Identifier, IComponentCollection> _componentsData;
    private readonly Identifier[] _components;
    private readonly Type?[] _dataTypes;
    private readonly IComponentCollection[] _data;

    internal Archetype(ArchetypeSharedData sharedData, IEnumerable<KeyValuePair<Identifier, Type?>> componentTypes)
    {
        _sharedData = sharedData;
        _componentDataTypes = componentTypes.ToImmutableSortedDictionary();
        _componentsData = _componentDataTypes.ToImmutableSortedDictionary(c => c.Key, c => _sharedData.CreateComponentCollection(c.Value));
        _components = [.._componentDataTypes.Keys];
        _dataTypes = [.._componentDataTypes.Values];
        _data = [.._componentsData.Values];
    }

    internal Archetype(ArchetypeSharedData sharedData)
    {
        _sharedData = sharedData;
        _componentDataTypes = ImmutableSortedDictionary<Identifier, Type?>.Empty;
        _componentsData = ImmutableSortedDictionary<Identifier, IComponentCollection>.Empty;
        _components = [];
        _dataTypes = [];
        _data = [];
    }

    /// <summary>
    /// All the components represented by this archetype
    /// </summary>
    public ReadOnlySpan<Identifier> Components => _components;

    /// <summary>
    /// All of the entities contained by this archetype.
    /// </summary>
    public ReadOnlySpan<Identifier> Entities => CollectionsMarshal.AsSpan(_entities);

    /// <summary>
    /// The amount of entities present in this archetype
    /// </summary>
    public int EntityCount => _entities.Count;

    /// <summary>
    /// Returns the collection of components for the given <paramref name="componentId"/>
    /// </summary>
    public IComponentCollection<T> GetComponentCollection<T>(Identifier componentId)
        => (IComponentCollection<T>)_data[_components.IndexOf(componentId)];

    /// <summary>
    /// Returns whether or not the archetype contains the given component
    /// </summary>
    public bool HasComponent(Identifier componentId) => _components.Contains(componentId);

    /// <summary>
    /// Adds the entity to the archetype and returns the index it has in the archetype
    /// </summary>
    public ArchetypeEntityEntry AddEntity(Identifier entityId)
    {
        foreach (var componentCollection in _data)
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
        foreach (var componentCollection in _data)
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
            targetArchetype = _sharedData.GetArchetype([.. _componentDataTypes, new(componentIdentifier, componentDataType)]);
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
            targetArchetype = _sharedData.GetArchetype(_componentDataTypes.Where(k => k.Key != componentId));
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

    /// <summary>
    /// Returns a boxed value of the component.
    /// </summary>
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
