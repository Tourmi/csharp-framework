using CurrentEntityNewArchetype = (Tourmi.EntityComponentSystem.Archetypes.Archetype NewArchetype, int NewIndex);
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
    private readonly Identifier[] _components;
    private readonly Type?[] _componentDataTypes;
    private readonly ComponentCollection[] _componentsData;

    private Archetype(SharedData sharedData, IEnumerable<(Identifier ComponentId, Type? ComponentDataType)> componentTypes)
    {
        _sharedData = sharedData;
        _components = [.. componentTypes.Select(s => s.ComponentId)];
        _componentDataTypes = [.. componentTypes.Select(s => s.ComponentDataType)];
        _componentsData = new ComponentCollection[_components.Length];
        for (var i = 0; i < _componentsData.Length; i++)
        {
            _componentsData[i] = _sharedData.CreateComponentCollection(_componentDataTypes[i]);
        }
    }

    /// <summary>
    /// Returns the components that this archetype represents.
    /// </summary>
    public IEnumerable<Identifier> Components => _components;

    /// <summary>
    /// Adds the entity to the archetype and returns the index it has in the archetype
    /// </summary>
    public int AddEntity(Identifier entityId)
    {
        foreach (var component in _componentsData)
        {
            component.AddEntry();
        }

        _entities.Add(entityId);

        return _entities.Count - 1;
    }

    /// <summary>
    /// Removes the entity at the given index from the archetype, 
    /// returning the entity that needed to be moved to complete the operation and its new index.
    /// </summary>
    public Option<MovedEntity> RemoveEntity(int index)
    {
        foreach (var component in _componentsData)
        {
            component.RemoveEntry(index);
        }

        var replaceIndex = _entities.Count - 1;
        if (index == replaceIndex)
        {
            _entities.RemoveAt(replaceIndex);
            return default;
        }

        var movedEntity = _entities[replaceIndex];
        _entities.RemoveAt(replaceIndex);
        _entities[index] = movedEntity;

        return (movedEntity, index);
    }

    public (CurrentEntityNewArchetype CurrentEntity, Option<MovedEntity> MovedEntity) AddComponent(int currentIndex, Identifier componentIdentifier, Type? componentDataType)
    {
        if (!_parentArchetypes.ContainsKey(componentIdentifier))
        {

        }

        // TODO: remove from current archetype (which may move an entity), add to new archetype, set all component values
        throw new NotImplementedException();
    }

    public (CurrentEntityNewArchetype CurrentEntity, Option<MovedEntity> MovedEntity) RemoveComponent(int currentIndex, Identifier componentIdentifier)
    {
        if (!_childArchetypes.ContainsKey(componentIdentifier))
        {

        }

        // TODO: remove from current archetype (which may move an entity), add to new archetype, set all component values
        throw new NotImplementedException();
    }

    public static Archetype Create() => new(new(), []);
}
