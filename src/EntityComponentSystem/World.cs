using System.Reflection;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Entity Component System, stores, updates, and allows for creating or querying entities.
/// </summary>
[Singleton]
public sealed class World : IEntityActions
{
    private readonly Dictionary<Type, Identifier> _typesToEntityIds = new()
    {
        [typeof(Component)] = FixedIds.ComponentIds.Component,
        [typeof(DataComponent)] = FixedIds.ComponentIds.DataComponent,
        [typeof(Name)] = FixedIds.ComponentIds.Name,
        [typeof(SystemComponent)] = FixedIds.ComponentIds.SystemComponent,
        [typeof(Schedule)] = FixedIds.ComponentIds.Schedule,
        [typeof(RelationDefinition)] = FixedIds.ComponentIds.RelationDefinition,
        [typeof(ChildOf)] = FixedIds.Relations.ChildOf,
        [typeof(IsA)] = FixedIds.Relations.IsA,
        [typeof(InstanceOf)] = FixedIds.Relations.InstanceOf,
        [typeof(Requires)] = FixedIds.Relations.Requires,
        [typeof(DependsOn)] = FixedIds.Relations.DependsOn,
        [typeof(SubscribedTo)] = FixedIds.Relations.SubscribedTo,
        [typeof(Events.Tick)] = FixedIds.Events.Tick,
    };
    private readonly Identifier[] _builtInRelationIds = new Identifier[FixedIds.Relations.RegionSize];
    private readonly Dictionary<Type, Query> _queryParamsToQuery = [];

    private readonly Lock _threadLock = new();

    /// <summary>
    /// The world's queued actions, which are executed once it is safe to do so.
    /// </summary>
    private readonly Stack<EntityActions> _queuedActions = new();

    /// <summary>
    /// The world's entity identifier collection.
    /// </summary>
    internal IdentifierCollection Ids { get; } = new();

    /// <summary>
    /// The world's archetype collection.
    /// </summary>
    internal EntityArchetypeCollection Archetypes { get; } = new();

    /// <inheritdoc/>
    World IEntityActions.World => this;

    /// <summary>
    /// Creates and initializes a new ECS world
    /// </summary>
    private World(WorldConfiguration configuration)
    {
        Ids.Reserve(FixedIds.CoreRegion);

        foreach (var region in configuration.ReservedIdentifierRegions.EmptyIfNull())
        {
            Ids.Reserve(region);
        }

        InitializeBaseComponents();
        InitializeRelations();
        InitializeSystems();

        // todo: initialize rest of built-in features here

        return;

        void InitializeBaseComponents()
        {
            var component = CreateEntityFixedId(FixedIds.ComponentIds.Component);
            Archetypes.AddComponent(component, component, null);

            var data = CreateEntityFixedId(FixedIds.ComponentIds.DataComponent);
            Archetypes.AddComponent(data, component, null);
            Archetypes.AddComponent(data, data, typeof(DataComponent));
            Archetypes.SetComponent(data, data, new DataComponent(typeof(DataComponent)));

            var name = CreateEntityFixedId(FixedIds.ComponentIds.Name);
            Archetypes.AddComponent(name, component, null);
            Set<DataComponent>(name, data, new(typeof(Name)));
            Set<Name>(name, name, new(nameof(Name)));

            Set<Name>(component, name, new(nameof(Component)));
            Set<Name>(data, name, new(nameof(DataComponent)));

            var singleton = CreateEntityFixedId(FixedIds.ComponentIds.Singleton);
            this.Add<Component>(singleton);
            this.Set<Name>(singleton, new(nameof(Singleton)));

            var world = CreateEntityFixedId(FixedIds.Entities.World);
            this.Add<Singleton>(world);
            this.Set<Name>(world, new(nameof(World)));
        }

        void InitializeRelations()
        {
            var relationDefinition = CreateEntityFixedId(FixedIds.ComponentIds.RelationDefinition);
            this.Add<Component>(relationDefinition);
            this.Set<Name>(relationDefinition, new(nameof(RelationDefinition)));
            this.Set<DataComponent>(relationDefinition, new(typeof(RelationDefinition)));

            for (ushort i = 0; i < _builtInRelationIds.Length; i++)
            {
                if (!Enum.IsDefined((BuiltInRelationType)(i)))
                {
                    continue;
                }

                var relation = CreateEntityFixedId(new Identifier(FixedIds.Relations.RegionStart + i) | IdentifierTypes.Relation);
                this.Add<Component>(relation);
                this.Set<Name>(relation, new(Enum.GetName((BuiltInRelationType)i) ?? $"Relation #{i}"));
                this.Set<RelationDefinition>(relation, new(i));
                _builtInRelationIds[i] = relation;
            }

            // For now, configure relations manually.
            this.Set<DataComponent>(FixedIds.Relations.Requires, new(typeof(Requires)));
        }

        void InitializeSystems()
        {
            var system = CreateEntityFixedId(FixedIds.ComponentIds.SystemComponent);
            this.Add<Component>(system);
            this.Set<DataComponent>(system, new(typeof(SystemComponent)));
            this.Set<Name>(system, new(nameof(SystemComponent)));

            var scheduleComponent = CreateEntityFixedId(FixedIds.ComponentIds.Schedule);
            this.Add<Component>(scheduleComponent);
            this.Set<DataComponent>(scheduleComponent, new(typeof(Schedule)));
            this.Set<Name>(scheduleComponent, new(nameof(Schedule)));
        }
    }

    /// <summary>
    /// Creates a new ECS <see cref="World"/>, optionally configuring it with <paramref name="configure"/>
    /// </summary>
    public static World Create(Action<WorldConfiguration>? configure = null)
    {
        var configuration = new WorldConfiguration();
        configure?.Invoke(configuration);

        return new World(configuration);
    }

    /// <inheritdoc/>
    public Entity CreateEntity()
    {
        lock (_threadLock)
        {
            var id = Ids.Create();
            Archetypes.Create(id);
            var entity = new Entity(id, this);

            return entity;
        }
    }

    /// <inheritdoc/>
    public bool IsAlive(Identifier entity) => Archetypes.IsAlive(entity);

    /// <inheritdoc/>
    public bool IsValid(Identifier id)
    {
        if (!id.Types.HasFlag(IdentifierTypes.Relation))
        {
            return Archetypes.IsAlive(id);
        }

        var relationId = new RelationComponentIdentifier(id);

        var relationIndex = relationId.RelationType;
        if (relationIndex == 0)
        {
            return false;
        }

        Identifier relationEntity;
        if (relationIndex < _builtInRelationIds.Length)
        {
            relationEntity = _builtInRelationIds[relationIndex];
        }
        else
        {
            // TODO: Get user-defined relations as well
            throw new NotImplementedException();
        }

        if (!Archetypes.IsAlive(relationEntity))
        {
            return false;
        }

        if (relationId.Target == FixedIds.Special.Wildcard.ShortId)
        {
            return true;
        }

        return Archetypes.IsAlive(relationId.Target);
    }

    /// <inheritdoc/>
    public void Kill(Identifier entity)
    {
        if (!Archetypes.IsAlive(entity))
        {
            return;
        }

        // Remove entity from entities (if it was used as a component)
        foreach (var archetype in Archetypes.ComponentsToArchetypes[entity])
        {
            while (archetype.EntityCount > 0)
            {
                Remove(archetype.Entities[0], entity);
            }
        }

        // TODO: Kill all relations where Target == entity
        // TODO: Kill all relations where RelationType == entity

        Archetypes.Kill(entity);

        if (!Ids.IsInUse(entity))
        {
            return;
        }

        Ids.Free(entity);
    }

    /// <inheritdoc/>
    public bool Has(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(componentId))
        {
            return false;
        }

        return Archetypes.HasComponent(entity, componentId);
    }

    /// <inheritdoc/>
    public T? Get<T>(Identifier entity, Identifier component)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(component))
        {
            return default;
        }

        if (!Archetypes.HasComponent(entity, component))
        {
            return default;
        }

        return Archetypes.GetComponent<T>(entity, component);
    }

    /// <inheritdoc/>
    public ref T? GetMutable<T>(Identifier entity, Identifier component)
    {
        if (!Archetypes.IsAlive(entity))
        {
            EntityInvalidException.ThrowEntityInvalid(entity);
        }

        if (!IsValid(component))
        {
            EntityInvalidException.ThrowComponentInvalid(component);
        }

        if (!Archetypes.HasComponent(entity, component))
        {
            EntityInvalidException.ThrowMissingComponent(entity, component);
        }

        return ref Archetypes.GetRefComponent<T>(entity, component);
    }

    /// <inheritdoc/>
    public void Add(Identifier entity, Identifier component)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        AddComponentIfMissing(entity, component);
    }

    /// <inheritdoc/>
    public void Set<T>(Identifier entity, Identifier component, T value)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        AddComponentIfMissing(entity, component);

        Archetypes.SetComponent(entity, component, value);
    }

    /// <inheritdoc/>
    public void Remove(Identifier entity, Identifier component)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        if (!Archetypes.HasComponent(entity, component))
        {
            return;
        }

        Archetypes.RemoveComponent(entity, component);
    }

    /// <summary>
    /// Returns the entity mapped to the given <paramref name="type"/>.
    /// </summary>
    public Entity GetEntityForType(Type type)
    {
        lock (_threadLock)
        {
            if (!_typesToEntityIds.TryGetValue(type, out var entityId) || !IsAlive(entityId))
            {
                Entity entity;
                if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Relation<,>))
                {
                    var genericArguments = type.GetGenericArguments();
                    var relationDefinitionType = genericArguments[0];
                    var targetType = genericArguments[1];

                    var relationDefinition = GetEntityForType(relationDefinitionType);
                    var targetEntity = GetEntityForType(targetType);
                    var relationType = relationDefinition.Get<RelationDefinition>().RelationType;
                    var targetId = targetEntity.Id;

                    entityId = new RelationComponentIdentifier(targetId.ShortId, relationType);
                    entity = CreateEntityFixedId(entityId);
                    entity.Set(new Name($"({relationDefinition.DisplayName} - {targetEntity.DisplayName})"));
                    _typesToEntityIds[type] = entity;

                    // Remaining setup is based on the relation definition.
                    type = relationDefinitionType;
                }
                else
                {
                    entity = CreateEntity();
                    entityId = entity.Id;
                    entity.Set(new Name(type.Name));
                    _typesToEntityIds[type] = entityId;
                }

                entity.Add<Component>();

                if (type.GetCustomAttribute<TagComponentAttribute>() is null)
                {
                    entity.Set(new DataComponent(type));
                }

                if (type.GetCustomAttribute<ComponentRelationTypeAttribute>() is not null)
                {
                    entity.Add<RelationDefinition>();
                }

                if (type.GetCustomAttribute<SingletonAttribute>() is not null)
                {
                    entity.Add<Singleton>();
                    entity.Add(entity.Id);
                }
            }

            return new(entityId, this);
        }
    }

    /// <summary>
    /// Attaches an existing entity to a type,
    /// such as when the <paramref name="type"/> is requested,
    /// the given <paramref name="entity"/> will be filled in.
    /// </summary>
    public void AttachEntityToType(Type type, Identifier entity)
    {
        lock (_threadLock)
        {
            _typesToEntityIds[type] = entity;
        }
    }

    internal EntityActions GetEntityActions()
    {
        lock (_threadLock)
        {
            if (!_queuedActions.TryPop(out var actions))
            {
                actions = new(this);
            }

            actions.DeferActions = true;

            return actions;
        }
    }

    internal void ReturnQueuedActions(EntityActions actions)
    {
        lock (_threadLock)
        {
            _queuedActions.Push(actions);
        }
    }

    internal void RunQueuedActions()
    {
        lock (_threadLock)
        {
            foreach (var actions in _queuedActions)
            {
                actions.DequeueActions();
                actions.DeferActions = false;
            }
        }
    }

    /// <summary>
    /// Returns the cached query for the given type.
    /// </summary>
    internal Query GetCachedQueryFor<T>()
        where T : IQueryParam<T>, allows ref struct
    {
        var queryType = typeof(T);

        if (!_queryParamsToQuery.TryGetValue(queryType, out var query))
        {
            query = Query.FromQueryParam<T>(this);
            _queryParamsToQuery[queryType] = query;
        }

        return query;
    }

    /// <summary>
    /// Adds the <paramref name="component"/> to the <paramref name="entity"/> if it is missing.
    /// </summary>
    private void AddComponentIfMissing(Identifier entity, Identifier component)
    {
        if (Archetypes.HasComponent(entity, component))
        {
            return;
        }

        var datatypeId = component;
        if (component.Types.HasFlag(IdentifierTypes.Relation))
        {
            var relationId = new RelationComponentIdentifier(component);
            if (relationId.BuiltInRelationTypeOrNull != null)
            {
                datatypeId = ToId(relationId.BuiltInRelationTypeOrNull.Value);
            }
            else
            {
                // TODO: Also process user-created relations.
                throw new NotImplementedException();
            }
        }

        var dataType = this.Get<DataComponent>(datatypeId);
        Archetypes.AddComponent(entity, component, dataType.DataType);
    }

    private Entity CreateEntityFixedId(Identifier id)
    {
        Archetypes.Create(id);
        return new(id, this);
    }

    private Identifier ToId(BuiltInRelationType relationType) => _builtInRelationIds[(int)relationType];
}
