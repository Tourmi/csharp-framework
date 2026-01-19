using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Performs actions on entities, which may be executed immediately, or later.
/// </summary>
public sealed class EntityActions : IEntityActions, IQueryParam<EntityActions>
{
    private enum ActionType
    {
        Add,
        Remove,
        Set,
        Kill,
    }

    private readonly Queue<ActionType> _queuedActions = [];
    private readonly Queue<(Identifier Entity, Identifier Component)> _queuedAdds = [];
    private readonly Queue<(Identifier Entity, Identifier Component)> _queuedRemoves = [];
    private readonly Queue<(Identifier Entity, Identifier Component)> _queuedSets = [];
    private readonly Queue<Identifier> _queuedKills = [];

    private readonly Dictionary<(Identifier Entity, Identifier Component), bool> _hasComponentOverrides = [];
    private readonly Dictionary<Identifier, bool> _isAliveOverrides = [];
    private readonly Dictionary<(Identifier Entity, Identifier Component), int> _componentValueOverrideIndexes = [];
    private readonly Dictionary<Identifier, IComponentCollection> _componentValueOverrides = [];

    internal EntityActions(World world)
    {
        World = world;
    }

    /// <summary>
    /// Whether or not future actions should be deferred.
    /// </summary>
    internal bool DeferActions { get; set; }

    /// <inheritdoc cref="IEntityActions.World"/>
    internal World World { get; }

    /// <inheritdoc/>
    World IEntityActions.World => World;

    /// <inheritdoc/>
    public Entity CreateEntity() => new(World.CreateEntity(), this);

    /// <inheritdoc/>
    public void Kill(Identifier entity)
    {
        if (!DeferActions)
        {
            World.Kill(entity);
            return;
        }

        _isAliveOverrides[entity] = false;
        _queuedActions.Enqueue(ActionType.Kill);
        _queuedKills.Enqueue(entity);
    }

    /// <inheritdoc/>
    public T? Get<T>(Identifier entity, Identifier component)
    {
        if (!Has(entity, component))
        {
            return default;
        }

        if (_componentValueOverrideIndexes.TryGetValue((entity, component), out var index))
        {
            var collection = GetComponentCollection<T>(component);
            return collection[index];
        }

        return World.Get<T>(entity, component);
    }

    /// <inheritdoc/>
    public ref T? GetMutable<T>(Identifier entity, Identifier component)
    {
        if (!Has(entity, component))
        {
            EntityInvalidException.ThrowMissingComponent(entity, component);
        }

        var collection = GetComponentCollection<T>(component);

        if (!_componentValueOverrideIndexes.TryGetValue((entity, component), out var index))
        {
            var value = World.Get<T>(entity, component);

            index = collection.Count;
            collection.AddEntry();

            _componentValueOverrideIndexes[(entity, component)] = index;
            collection[index] = value;
        }

        return ref collection[index];
    }

    /// <inheritdoc/>
    public bool Has(Identifier entity, Identifier componentId)
    {
        if (_hasComponentOverrides.TryGetValue((entity, componentId), out var hasComponent))
        {
            return hasComponent;
        }

        return World.Has(entity, componentId);
    }

    /// <inheritdoc/>
    public bool IsAlive(Identifier entity)
    {
        if (_isAliveOverrides.TryGetValue(entity, out var isAlive))
        {
            return isAlive;
        }

        return World.IsAlive(entity);
    }

    /// <inheritdoc/>
    public bool IsValid(Identifier id)
    {
        if (_isAliveOverrides.TryGetValue(id, out var isAlive))
        {
            return isAlive;
        }

        return World.IsValid(id);
    }

    /// <inheritdoc/>
    public void Add(Identifier entity, Identifier component)
    {
        if (!DeferActions)
        {
            World.Add(entity, component);
            return;
        }

        _queuedActions.Enqueue(ActionType.Add);
        _queuedAdds.Enqueue((entity, component));
        _hasComponentOverrides[(entity, component)] = true;
    }

    /// <inheritdoc/>
    public void Remove(Identifier entity, Identifier component)
    {
        if (!DeferActions)
        {
            World.Remove(entity, component);
            return;
        }

        _queuedActions.Enqueue(ActionType.Remove);
        _queuedRemoves.Enqueue((entity, component));
        _hasComponentOverrides[(entity, component)] = false;
    }

    /// <inheritdoc/>
    public void Set<T>(Identifier entity, Identifier component, T value)
    {
        if (!DeferActions)
        {
            World.Set(entity, component, value);
            return;
        }

        if (!Has(entity, component))
        {
            Add(entity, component);
        }

        var collection = GetComponentCollection<T>(component);
        if (!_componentValueOverrideIndexes.TryGetValue((entity, component), out var componentIndex))
        {
            componentIndex = collection.Count;
            collection.AddEntry();

            _componentValueOverrideIndexes[(entity, component)] = componentIndex;
        }

        collection[componentIndex] = value;

        _queuedActions.Enqueue(ActionType.Set);
        _queuedSets.Enqueue((entity, component));
    }

    /// <summary>
    /// Dequeues all actions that have been queued.
    /// </summary>
    internal void DequeueActions()
    {
        while (_queuedActions.TryDequeue(out var actionType))
        {
            switch (actionType)
            {
                case ActionType.Add:
                    OnAdd();
                    break;
                case ActionType.Remove:
                    OnRemove();
                    break;
                case ActionType.Set:
                    OnSet();
                    break;
                case ActionType.Kill:
                    OnKill();
                    break;
                default: throw new NotSupportedException();
            }
        }

        _componentValueOverrideIndexes.Clear();
        _hasComponentOverrides.Clear();
        _isAliveOverrides.Clear();

        foreach (var collection in _componentValueOverrides.Values)
        {
            collection.Clear();
        }

        return;

        void OnAdd()
        {
            var action = _queuedAdds.Dequeue();
            World.Add(action.Entity, action.Component);
        }

        void OnRemove()
        {
            var action = _queuedRemoves.Dequeue();
            World.Remove(action.Entity, action.Component);
        }

        void OnSet()
        {
            var action = _queuedSets.Dequeue();
            var entry = World.Archetypes.GetArchetypeEntry(action.Entity);
            if (!entry.HasValue)
            {
                return;
            }

            var sourceCollection = _componentValueOverrides[action.Component];
            var sourceIndex = _componentValueOverrideIndexes[action];

            var targetCollection = entry.Value.Archetype.GetComponentCollection(action.Component);
            targetCollection.CopyValueFrom(sourceCollection, sourceIndex, targetIndex: entry.Value.Index);
        }

        void OnKill()
        {
            var action = _queuedKills.Dequeue();
            World.Kill(action);
        }
    }

    private IComponentCollection<T> GetComponentCollection<T>(Identifier component)
    {
        if (!_componentValueOverrides.TryGetValue(component, out var collection))
        {
            collection = new ComponentCollection<T>();
            _componentValueOverrides[component] = collection;
        }

        return (IComponentCollection<T>)collection;
    }

    static QueryParamGlobalCache IQueryParam<EntityActions>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParam<EntityActions>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static EntityActions IQueryParam<EntityActions>.CreateFrom(QueryParamEntityInfo entry)
        => QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache);
}
