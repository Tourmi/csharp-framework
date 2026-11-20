using System.Reflection;
using Tourmi.EntityComponentSystem.Queries.Parameters;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Entity Component System, stores, updates, and allows for creating or querying entities.
/// </summary>
[Singleton]
public sealed class World : IEntityActions
{
    private readonly Dictionary<Type, Identifier> _typesToEntityIds = new()
    {
        [typeof(World)] = FixedIds.Entities.World,

        [typeof(Component)] = FixedIds.Components.Component,
        [typeof(DataComponent)] = FixedIds.Components.DataComponent,
        [typeof(Name)] = FixedIds.Components.Name,
        [typeof(RelationDefinition)] = FixedIds.Components.RelationDefinition,
        [typeof(Singleton)] = FixedIds.Components.Singleton,
        [typeof(Prefab)] = FixedIds.Components.Prefab,
        [typeof(Schedule)] = FixedIds.Components.Schedule,
        [typeof(SystemComponent)] = FixedIds.Components.SystemComponent,
        [typeof(Unique)] = FixedIds.Components.Unique,
        [typeof(Symmetric)] = FixedIds.Components.Symmetric,
        [typeof(Transitive)] = FixedIds.Components.Transitive,
        [typeof(Reflective)] = FixedIds.Components.Reflective,
        [typeof(EventActionSystem)] = FixedIds.Components.EventActionSystem,

        [typeof(ChildOf)] = FixedIds.Relations.ChildOf,
        [typeof(IsA)] = FixedIds.Relations.IsA,
        [typeof(InstanceOf)] = FixedIds.Relations.InstanceOf,
        [typeof(Requires)] = FixedIds.Relations.Requires,
        [typeof(DependsOn)] = FixedIds.Relations.DependsOn,
        [typeof(SubscribedTo)] = FixedIds.Relations.SubscribedTo,
        [typeof(Wildcard)] = FixedIds.Relations.Wildcard,

        [typeof(Events.PreTick)] = FixedIds.Events.PreTick,
        [typeof(Events.Tick)] = FixedIds.Events.Tick,
        [typeof(Events.PostTick)] = FixedIds.Events.PostTick,
        [typeof(OnAdd)] = FixedIds.Events.OnAdd,
        [typeof(OnModified)] = FixedIds.Events.OnModified,
        [typeof(OnRemove)] = FixedIds.Events.OnRemove,
        [typeof(OnKill)] = FixedIds.Events.OnKill,
        [typeof(OnTargetKill)] = FixedIds.Events.OnTargetKill,
        [typeof(OnExtend)] = FixedIds.Events.OnExtend,

        [typeof(Remove)] = FixedIds.Actions.Remove,
        [typeof(Kill)] = FixedIds.Actions.Kill,
        [typeof(Panic)] = FixedIds.Actions.Panic,
        [typeof(Copy)] = FixedIds.Actions.Copy,
        [typeof(Inherit)] = FixedIds.Actions.Inherit,
        [typeof(Ignore)] = FixedIds.Actions.Ignore,

        [typeof(Parameter1)] = FixedIds.Special.Parameter1,
        [typeof(Parameter2)] = FixedIds.Special.Parameter2,
        [typeof(Parameter3)] = FixedIds.Special.Parameter3,
        [typeof(Parameter4)] = FixedIds.Special.Parameter4,
        [typeof(Parameter5)] = FixedIds.Special.Parameter5,
        [typeof(Parameter6)] = FixedIds.Special.Parameter6,
        [typeof(Parameter7)] = FixedIds.Special.Parameter7,
        [typeof(Parameter8)] = FixedIds.Special.Parameter8,
    };
    private readonly LazyPagedArray<Identifier> _relationDefinitionIds = new(pageSizePowerOf2: 10);
    private readonly Dictionary<Type, Query> _queryParamsToQuery = [];
    private readonly Dictionary<(Type, ulong), Query> _parametrized1Queries = [];

    private readonly Lock _threadLock = new();

    /// <summary>
    /// The pooled entity action instances.
    /// </summary>
    private readonly AutoPool<EntityActions> _entityActionsPool;

    /// <summary>
    /// The world's queued actions, which are executed once it is safe to do so.
    /// </summary>
    private readonly Queue<EntityActions> _queuedEntityActions = new();

    private ushort _currentRelationId = 256;

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
        _entityActionsPool = new(() => new EntityActions(this));
        Ids.Reserve(FixedIds.CoreRegion);

        foreach (var region in configuration.ReservedIdentifierRegions.EmptyIfNull())
        {
            Ids.Reserve(region);
        }

        // Initialize _relationDefinitionIds collection before doing anything else.
        for (ushort i = 0; i < FixedIds.Relations.RegionSize; i++)
        {
            if (!Enum.IsDefined((BuiltInRelationType)i))
            {
                continue;
            }

            var relation = CreateEntityFixedId(FixedIds.Relations.RegionStart + i);
            _relationDefinitionIds[i] = relation;
        }

        InitializeBaseComponents();
        InitializeRelations();
        InitializeAdditionalComponents();
        InitializeSystems();

        return;

        void InitializeBaseComponents()
        {
            var component = CreateEntityFixedId(FixedIds.Components.Component);
            Archetypes.AddComponent(component, component, null);

            var data = CreateEntityFixedId(FixedIds.Components.DataComponent);
            Archetypes.AddComponent(data, component, null);
            Archetypes.AddComponent(data, data, typeof(DataComponent));
            Archetypes.SetComponent(data, data, new DataComponent(typeof(DataComponent)));

            var name = CreateEntityFixedId(FixedIds.Components.Name);
            Archetypes.AddComponent(name, component, null);
            Set<DataComponent>(name, data, new(typeof(Name)));
            Set<Name>(name, name, new(nameof(Name)));

            Set<Name>(component, name, new(nameof(Component)));
            Set<Name>(data, name, new(nameof(DataComponent)));
        }

        void InitializeRelations()
        {
            var relationDefinition = CreateEntityFixedId(FixedIds.Components.RelationDefinition);
            Add(relationDefinition, FixedIds.Components.Component);
            Set<Name>(relationDefinition, FixedIds.Components.Name, new(nameof(RelationDefinition)));
            Set<DataComponent>(relationDefinition, FixedIds.Components.DataComponent, new(typeof(RelationDefinition)));

            for (ushort i = 0; i < FixedIds.Relations.RegionSize; i++)
            {
                if (!Enum.IsDefined((BuiltInRelationType)(i)))
                {
                    continue;
                }

                var relation = _relationDefinitionIds[i];
                Add(relation, FixedIds.Components.Component);
                Set<Name>(relation, FixedIds.Components.Name, new(Enum.GetName((BuiltInRelationType)i) ?? $"Relation #{i}"));
                Set<RelationDefinition>(relation, FixedIds.Components.RelationDefinition, new(i));
            }

            // For now, configure relations manually.
            this.Set<DataComponent>(FixedIds.Relations.Requires, new(typeof(Requires)));

            this.Add<Unique>(FixedIds.Relations.InstanceOf);
            this.Add<Unique>(FixedIds.Relations.ChildOf);

            this.Add<Transitive>(FixedIds.Relations.IsA);

            this.Add<Reflective>(FixedIds.Relations.IsA);

            this.Add<Relation<OnExtend, Ignore>>(FixedIds.Relations.IsA);
        }

        void InitializeAdditionalComponents()
        {
            var singleton = CreateEntityFixedId(FixedIds.Components.Singleton);
            this.Add<Component>(singleton);
            this.Set<Name>(singleton, new(nameof(Singleton)));

            var world = CreateEntityFixedId(FixedIds.Entities.World);
            this.Add<Singleton>(world);
            this.Add(world, world);
            this.Set<Name>(world, new(nameof(World)));
        }

        void InitializeSystems()
        {
            var system = CreateEntityFixedId(FixedIds.Components.SystemComponent);
            this.Add<Component>(system);
            this.Set<DataComponent>(system, new(typeof(SystemComponent)));
            this.Set<Name>(system, new(nameof(SystemComponent)));

            var scheduleComponent = CreateEntityFixedId(FixedIds.Components.Schedule);
            this.Add<Component>(scheduleComponent);
            this.Set<DataComponent>(scheduleComponent, new(typeof(Schedule)));
            this.Set<Name>(scheduleComponent, new(nameof(Schedule)));

            var removeSystem = CreateEntityFixedId(FixedIds.Actions.Remove);
            this.Set<Name>(removeSystem, new("Remove Component action"));
            this.Set<EventActionSystem>(removeSystem, new((world, eventId, entityId, componentId) =>
            {
                world.Remove(entityId, componentId);
            }));

            var killSystem = CreateEntityFixedId(FixedIds.Actions.Kill);
            this.Set<Name>(killSystem, new("Kill action"));
            this.Set<EventActionSystem>(killSystem, new((world, eventId, entityId, componentId) =>
            {
                world.Kill(entityId);
            }));

            var panicSystem = CreateEntityFixedId(FixedIds.Actions.Panic);
            this.Set<Name>(panicSystem, new("Panic action"));
            this.Set<EventActionSystem>(panicSystem, new((world, eventId, entityId, componentId) =>
            {
                var eventEntity = new Entity(eventId, world);
                var entity = new Entity(entityId, world);
                var component = new Entity(componentId, world);

                throw new InvalidOperationException(
                    $"An invalid action ({eventEntity.DisplayName}) " +
                    $"was performed on the component {component.DisplayName}, " +
                    $"panicking due to entity {entity.DisplayName}.");
            }));

            var relationDefinitionComponent = new Entity(FixedIds.Components.RelationDefinition, this);
            this.Set<EventActionSystem>(relationDefinitionComponent, new((world, eventId, entityId, componentId) =>
            {
                if (eventId != FixedIds.Events.OnAdd)
                {
                    return;
                }

                ref var relationDefinition = ref world.GetMutable<RelationDefinition>(entityId);

                var relationType = relationDefinition.RelationType;
                while (relationType == 0)
                {
                    if (_currentRelationId == 0)
                    {
                        throw new InvalidOperationException("Cannot create new relation type, ran out of ids");
                    }

                    if (_relationDefinitionIds[_currentRelationId] == default)
                    {
                        relationType = _currentRelationId;
                    }

                    _currentRelationId++;
                }

                relationDefinition = new(relationType);
                _relationDefinitionIds[relationDefinition.RelationType] = entityId;
            }));
            relationDefinitionComponent.AddRelation(BuiltInRelationType.OnAdd, relationDefinitionComponent);
            relationDefinitionComponent.AddRelation(BuiltInRelationType.OnRemove, relationDefinitionComponent);

            var singletonComponent = new Entity(FixedIds.Components.Singleton, this);
            this.Set<EventActionSystem>(singletonComponent, new((world, eventId, entityId, componentId) =>
            {
                var entity = new Entity(entityId, world);
                if (eventId == FixedIds.Events.OnAdd)
                {
                    entity.Add(entity);
                }
                else if (eventId == FixedIds.Events.OnRemove)
                {
                    entity.Remove(entity);
                }
            }));
            singletonComponent.AddRelation(BuiltInRelationType.OnAdd, singletonComponent);
            singletonComponent.AddRelation(BuiltInRelationType.OnRemove, singletonComponent);

            this.Add<Relation<OnTargetKill, Kill>>(FixedIds.Relations.ChildOf);
            this.Add<Relation<OnTargetKill, Kill>>(FixedIds.Relations.IsA);

            var isAComponent = new Entity(FixedIds.Relations.IsA, this);
            this.Set<EventActionSystem>(isAComponent, new((world, eventId, entityId, componentId) =>
            {
                if (eventId == FixedIds.Events.OnAdd && componentId.IsRelationId)
                {
                    var superEntityId = Ids.GetCurrentId(componentId.ShortId);
                    var superArchetype = Archetypes.GetEntityArchetype(superEntityId);
                    if (superArchetype is null)
                    {
                        return;
                    }

                    for (var i = 0; i < superArchetype.Components.Length; i++)
                    {
                        var superComponentId = superArchetype.Components[i];
                        var superComponentDefinition = GetComponentDefinition(superComponentId);
                        if (superComponentDefinition.Has(FixedIds.Events.OnExtendIgnore) || superComponentDefinition.Has(FixedIds.Events.OnExtendInherit))
                        {
                            continue;
                        }

                        CopyValueFrom(entityId, superEntityId, superComponentId);
                    }

                    // unsure if we need to notify queries here
                }
                else if (eventId == FixedIds.Events.OnRemove)
                {
                    // unsure if we need to notify queries here
                }
            }));
            isAComponent.AddRelation(BuiltInRelationType.OnAdd, isAComponent);
            isAComponent.AddRelation(BuiltInRelationType.OnRemove, isAComponent);

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
    public Entity CreateEntity(Identifier id, IdentifierRegion region)
    {
        if (!Ids.IsReserved(region))
        {
            throw new InvalidOperationException($"Cannot create an entity with id {id}, as the region {region} was never reserved.");
        }

        if (!region.Contains(id))
        {
            throw new IdentifierInvalidException(id, $"Given id {id} was not part of the given region {region}");
        }

        lock (_threadLock)
        {
            if (Archetypes.IsAlive(id))
            {
                IdentifierInvalidException.ThrowIdentifierAlreadyInUse(id);
            }

            Archetypes.Create(id);
            var entity = new Entity(id, this);

            return entity;
        }
    }

    /// <inheritdoc/>
    public bool IsAlive(Identifier entity) => Archetypes.IsAlive(entity);

    /// <inheritdoc/>
    public bool IsValid(Identifier id, bool allowWildcard = true)
    {
        if (!id.Types.HasFlag(IdentifierTypes.Relation))
        {
            return Archetypes.IsAlive(id);
        }

        var relationId = new RelationComponentIdentifier(id);

        var relationType = relationId.RelationType;
        if (relationType == 0)
        {
            return false;
        }

        if (relationId.BuiltInRelationTypeOrNull is BuiltInRelationType.Wildcard)
        {
            if (!allowWildcard)
            {
                return false;
            }
        }
        else
        {
            var relationDefinition = _relationDefinitionIds[relationType];
            if (!Archetypes.IsAlive(relationDefinition))
            {
                return false;
            }
        }

        if (relationId.Target == FixedIds.Relations.Wildcard.ShortId)
        {
            return allowWildcard;
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

        // Remove entity from entities
        foreach (var componentId in Archetypes.ComponentsToArchetypes.Components)
        {
            Identifier eventId = default;
            Identifier actionId = default;
            if (componentId == entity)
            {
                var component = new Entity(componentId, this);
                eventId = FixedIds.Events.OnKill;
                actionId = component.GetRelationTarget<OnKill>();

            }
            else if (componentId.IsRelationId && componentId.ShortId == entity.ShortId)
            {
                // Entity is part of a Relation<*, Entity>
                var relationId = new RelationComponentIdentifier(componentId);
                eventId = FixedIds.Events.OnTargetKill;

                var relationDefinition = new Entity(_relationDefinitionIds[relationId.RelationType], this);
                actionId = relationDefinition.GetRelationTarget<OnTargetKill>();
            }
            else
            {
                continue;
            }

            if (!IsValid(actionId))
            {
                actionId = FixedIds.Actions.Remove;
            }

            var actionEntity = new Entity(actionId, this);
            var cleanupAction = actionEntity.Get<EventActionSystem>().Action;

            foreach (var archetype in Archetypes.ComponentsToArchetypes[componentId])
            {
                // Always process entities of archetype in reverse order to save on swap operations.
                for (var i = archetype.EntityCount - 1; i >= 0; i = archetype.EntityCount - 1)
                {
                    cleanupAction(this, eventId, archetype.Entities[i], componentId);
                }
            }
        }

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
        if (Owns(entity, componentId))
        {
            return true;
        }

        var componentDefinition = GetComponentDefinition(componentId);
        if (componentDefinition.Owns(FixedIds.Events.OnExtendInherit))
        {
            var entityArchetype = Archetypes.GetArchetypeEntry(entity);
            if (!entityArchetype.HasValue)
            {
                throw new InvalidOperationException("Fatal error, entity somehow does not have an archetype.");
            }

            foreach (var component in entityArchetype.Value.Archetype.Components)
            {
                if (!component.IsRelationId || new RelationComponentIdentifier(component).BuiltInRelationTypeOrNull is not BuiltInRelationType.IsA)
                {
                    continue;
                }

                var extendedId = Ids.GetCurrentId(component);
                if (Has(extendedId, componentId))
                {
                    return true;
                }
            }
        }

        if (componentId.IsRelationId)
        {
            var relationId = componentId.ToRelationId();
            if (relationId.Target == entity.ShortId && componentDefinition.Has<Reflective>())
            {
                return true;
            }

            if (componentDefinition.Owns(FixedIds.Components.Transitive))
            {
                var archetype = Archetypes.GetEntityArchetype(entity);
                if (archetype is not null)
                {
                    foreach (var targetComponent in archetype.Components)
                    {
                        if (!targetComponent.IsRelationId)
                        {
                            continue;
                        }

                        var targetRelation = new RelationComponentIdentifier(targetComponent);
                        if (targetRelation.RelationType != relationId.RelationType)
                        {
                            continue;
                        }

                        if (TransitiveRelationsMatch(targetRelation, relationId))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Owns(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity))
        {
            return false;
        }

        if (!IsValid(componentId, allowWildcard: true))
        {
            return false;
        }

        return Archetypes.HasComponent(entity, componentId);
    }

    /// <inheritdoc/>
    public T? Get<T>(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(componentId, allowWildcard: false))
        {
            return default;
        }

        if (Archetypes.HasComponent(entity, componentId))
        {
            return Archetypes.GetComponent<T>(entity, componentId);
        }

        var componentDefinition = GetComponentDefinition(componentId);
        if (Has(componentDefinition, FixedIds.Events.OnExtendInherit))
        {
            var extendedId = RecursiveGetEntityWithComponent(entity, componentId);
            if (extendedId.HasValue)
            {
                return Archetypes.GetComponent<T>(extendedId.Get(), componentId);
            }
        }

        return default;
    }

    private Option<Identifier> RecursiveGetEntityWithComponent(Identifier entityId, Identifier componentId)
    {
        var entityArchetype = Archetypes.GetArchetypeEntry(entityId);
        if (!entityArchetype.HasValue)
        {
            throw new InvalidOperationException("Fatal error, entity somehow does not have an archetype.");
        }

        foreach (var component in entityArchetype.Value.Archetype.Components)
        {
            if (!component.IsRelationId || new RelationComponentIdentifier(component).BuiltInRelationTypeOrNull is not BuiltInRelationType.IsA)
            {
                continue;
            }

            var extendedId = Ids.GetCurrentId(component);
            if (Archetypes.HasComponent(extendedId, componentId))
            {
                return extendedId;
            }

            var recursiveGet = RecursiveGetEntityWithComponent(extendedId, componentId);
            if (recursiveGet.HasValue)
            {
                return recursiveGet;
            }
        }

        return default;
    }

    /// <inheritdoc/>
    public ref T? GetMutable<T>(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity))
        {
            EntityInvalidException.ThrowEntityInvalid(entity);
        }

        if (!IsValid(componentId, allowWildcard: false))
        {
            EntityInvalidException.ThrowComponentInvalid(componentId);
        }

        if (Archetypes.HasComponent(entity, componentId))
        {
            return ref Archetypes.GetRefComponent<T>(entity, componentId);
        }

        var componentDefinition = GetComponentDefinition(componentId);
        if (Has(componentDefinition, FixedIds.Events.OnExtendInherit))
        {
            var extendedId = RecursiveGetEntityWithComponent(entity, componentId);
            if (extendedId.HasValue)
            {
                return ref Archetypes.GetRefComponent<T>(extendedId.Get(), componentId);
            }
        }

        EntityInvalidException.ThrowMissingComponent(entity, componentId);
        throw new InvalidOperationException(); // line unreachable
    }

    /// <inheritdoc/>
    public void Add(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(componentId, allowWildcard: false))
        {
            return;
        }

        if (!AddComponentIfMissing(entity, componentId))
        {
            return;
        }

        OnAdd(entity, componentId);
        OnModified(entity, componentId, skipThisEntity: true);
    }

    /// <summary>
    /// Copies the <paramref name="componentId"/>'s component value from the <paramref name="sourceEntity"/> into the given <paramref name="entity"/>,
    /// adding the component if it is missing.
    /// </summary>
    public void CopyValueFrom(Identifier entity, Identifier sourceEntity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity) || !Archetypes.IsAlive(sourceEntity) || !IsValid(componentId, allowWildcard: false))
        {
            return;
        }

        if (!Has(sourceEntity, componentId))
        {
            return;
        }

        var wasAdded = AddComponentIfMissing(entity, componentId);
        Archetypes.CopyValueFrom(entity, sourceEntity, componentId);

        if (wasAdded)
        {
            OnAdd(entity, componentId);
        }

        OnModified(entity, componentId);
    }

    /// <inheritdoc/>
    public void Set<T>(Identifier entity, Identifier componentId, T value)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(componentId, allowWildcard: false))
        {
            return;
        }

        var wasAdded = AddComponentIfMissing(entity, componentId);
        Archetypes.SetComponent(entity, componentId, value);

        if (wasAdded)
        {
            OnAdd(entity, componentId);
        }

        OnModified(entity, componentId);
    }

    /// <inheritdoc/>
    public void Modified(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(componentId, allowWildcard: false))
        {
            return;
        }

        if (!Has(entity, componentId))
        {
            return;
        }

        OnModified(entity, componentId);
    }

    /// <inheritdoc/>
    public void Remove(Identifier entity, Identifier componentId)
    {
        if (!Archetypes.IsAlive(entity) || !IsValid(componentId, allowWildcard: false))
        {
            return;
        }

        if (!Archetypes.HasComponent(entity, componentId))
        {
            return;
        }

        OnRemove(entity, componentId);
        Archetypes.RemoveComponent(entity, componentId);

        if (componentId.IsRelationId)
        {
            var relationId = componentId.ToRelationId();
            var componentEntity = new Entity(_relationDefinitionIds[relationId.RelationType], this);
            if (componentEntity.Has<Symmetric>())
            {
                var target = Ids.GetCurrentId(relationId.Target);
                Remove(target, relationId.WithTarget(entity));
            }
        }

        if (Has(entity, componentId))
        {
            // Since the entity still has the component,
            // it means we removed the override, but it still inherits it from a base entity.
            // Raise OnModified since the value between itself and its base entity may be different.
            OnModified(entity, componentId);
        }
    }

    /// <summary>
    /// Returns the id of the definition of the given <paramref name="relationType"/>.
    /// </summary>
    public Entity GetRelationDefinition(ushort relationType) => new(_relationDefinitionIds[relationType], this);

    /// <inheritdoc/>
    public Identifier GetRelationTarget(Identifier entity, ushort relationType)
    {
        if (!Archetypes.IsAlive(entity))
        {
            return default;
        }

        var archetype = Archetypes.GetEntityArchetype(entity);
        if (archetype is null)
        {
            return default;
        }

        foreach (var component in archetype.Components)
        {
            if (component.IsRelationId && new RelationComponentIdentifier(component).RelationType == relationType)
            {
                return Ids.GetCurrentId(component);
            }
        }

        return default;
    }

    /// <inheritdoc/>
    public Identifier GetRelationTarget(RelationComponentIdentifier relation)
    {
        if (!IsValid(relation))
        {
            return default;
        }

        return Ids.GetCurrentId(relation.Target);
    }

    /// <summary>
    /// Returns the entity mapped to the given <paramref name="type"/>.
    /// </summary>
    public Entity GetEntityForType(Type type)
    {
        _ = type.ThrowIfNull();

        if (!_typesToEntityIds.TryGetValue(type, out var entityId) || !IsValid(entityId, allowWildcard: true))
        {
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
                AttachEntityToType(type, entityId);

                return new Entity(entityId, this);
            }

            entityId = InitializeComponentForType(entityId, type);
        }

        return new(entityId, this);
    }

    /// <summary>
    /// Attaches an existing entity to a type,
    /// such as when the <paramref name="type"/> is requested,
    /// the given <paramref name="entityId"/> will be filled in.
    /// </summary>
    public void AttachEntityToType(Type type, Identifier entityId)
    {
        lock (_threadLock)
        {
            _typesToEntityIds[type] = entityId;
        }
    }

    /// <summary>
    /// Returns the cached query for the given type <typeparamref name="T"/>.
    /// </summary>
    public Query GetCachedQueryFor<T>()
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
    /// Returns the cached query for the given type <typeparamref name="T"/> and <paramref name="parameter1"/> value.
    /// </summary>
    public Query GetCachedQueryFor<T>(ulong parameter1)
        where T : IQueryParam<T>, allows ref struct
    {
        var queryType = typeof(T);

        if (!_parametrized1Queries.TryGetValue((queryType, parameter1), out var query))
        {
            query = Query.FromQueryParam<T>(this, parameter1);
            _parametrized1Queries[(queryType, parameter1)] = query;
        }

        return query;
    }

    internal EntityActions GetEntityActions()
    {
        EntityActions? actions;
        lock (_threadLock)
        {
            actions = _entityActionsPool.Pop();
        }

        actions.DeferActions = true;
        return actions;
    }

    internal void ReturnQueuedActions(EntityActions actions)
    {
        lock (_threadLock)
        {
            _queuedEntityActions.Enqueue(actions);
        }
    }

    internal void RunQueuedActions()
    {
        EntityActions? actions;

        // TODO: Add infinite loop detection, in case systems are stuck
        // in an event loop and keep queueing more and more actions.
        while (true)
        {
            lock (_threadLock)
            {
                if (!_queuedEntityActions.TryDequeue(out actions))
                {
                    return;
                }
            }

            actions.DequeueActions();
            actions.DeferActions = false;
            lock (_threadLock)
            {
                _entityActionsPool.Push(actions);
            }
        }
    }

    internal bool TransitiveRelationsMatch(RelationComponentIdentifier relation, RelationComponentIdentifier relationToMatch)
    {
        if (relation == relationToMatch)
        {
            return true;
        }

        var target = GetRelationTarget(relation);
        var targetArchetype = Archetypes.GetEntityArchetype(target);
        if (targetArchetype is null)
        {
            return false;
        }

        foreach (var targetComponent in targetArchetype.Components)
        {
            if (!targetComponent.IsRelationId)
            {
                continue;
            }

            var targetRelation = new RelationComponentIdentifier(targetComponent);
            if (targetRelation.RelationType != relationToMatch.RelationType)
            {
                continue;
            }

            if (TransitiveRelationsMatch(targetRelation, relationToMatch))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// If <paramref name="componentId"/> is a relation id, returns the relation definition, otherwise returns the original <paramref name="componentId"/>.
    /// </summary>
    internal Entity GetComponentDefinition(Identifier componentId)
    {
        var componentDefinitionId = componentId;
        if (componentId.IsRelationId)
        {
            componentDefinitionId = _relationDefinitionIds[componentId.ToRelationId().RelationType];
        }

        return new Entity(componentDefinitionId, this);
    }

    private Identifier InitializeComponentForType(Identifier entityId, Type type)
    {
        var entity = entityId.ShortId != 0 ? CreateEntityFixedId(entityId) : CreateEntity();
        entityId = entity.Id;
        entity.Set(new Name(type.Name));
        AttachEntityToType(type, entityId);

        entity.Add<Component>();

        if (type.GetCustomAttribute<TagComponentAttribute>() is null)
        {
            entity.Set(new DataComponent(type));
        }

        var relationTypeAttribute = type.GetCustomAttribute<ComponentRelationTypeAttribute>();
        if (relationTypeAttribute is not null)
        {
            entity.Set(new RelationDefinition(relationTypeAttribute.RelationType));

            if (relationTypeAttribute.IsUnique)
            {
                entity.Add<Unique>();
            }

            if (relationTypeAttribute.IsReflective)
            {
                entity.Add<Reflective>();
            }

            if (relationTypeAttribute.IsTransitive)
            {
                entity.Add<Transitive>();
            }

            if (relationTypeAttribute.IsSymmetric)
            {
                entity.Add<Symmetric>();
            }
        }

        if (type.GetCustomAttribute<SingletonAttribute>() is not null)
        {
            entity.Add<Singleton>();
        }

        return entityId;
    }

    /// <summary>
    /// Adds the <paramref name="component"/> to the <paramref name="entity"/> if it is missing.
    /// </summary>
    private bool AddComponentIfMissing(Identifier entity, Identifier component)
    {
        if (Archetypes.HasComponent(entity, component))
        {
            return false;
        }

        var definitionId = component;
        if (component.IsRelationId)
        {
            var relationId = new RelationComponentIdentifier(component);
            definitionId = _relationDefinitionIds[relationId.RelationType];
            if (Has(definitionId, FixedIds.Components.Unique) && Owns(entity, relationId.WithTarget(FixedIds.Relations.Wildcard.ShortId)))
            {
                var target = GetRelationTarget(entity, relationId.RelationType);
                Remove(entity, relationId.WithTarget(target.ShortId));
            }
        }

        var dataType = Get<DataComponent>(definitionId, FixedIds.Components.DataComponent);
        Archetypes.AddComponent(entity, component, dataType.DataType);

        return true;
    }

    private void OnAdd(Identifier entity, Identifier component)
    {
        var componentDefinition = GetComponentDefinition(component);
        var onAddActionId = GetRelationTarget(componentDefinition, (ushort)BuiltInRelationType.OnAdd);
        if (onAddActionId != default)
        {
            var action = Get<EventActionSystem>(onAddActionId, FixedIds.Components.EventActionSystem).Action;
            if (action is not null)
            {
                action(this, FixedIds.Events.OnAdd, entity, component);
            }
        }

        if (component.IsRelationId)
        {
            var relationId = component.ToRelationId();
            if (componentDefinition.Has<Symmetric>())
            {
                var target = Ids.GetCurrentId(relationId.Target);
                Add(target, relationId.WithTarget(entity));
            }
        }
    }

    private void OnRemove(Identifier entity, Identifier component)
    {
        var componentDefinition = GetComponentDefinition(component);
        var onRemoveActionId = GetRelationTarget(componentDefinition, (ushort)BuiltInRelationType.OnRemove);
        if (onRemoveActionId != default)
        {
            var action = Get<EventActionSystem>(onRemoveActionId, FixedIds.Components.EventActionSystem).Action;
            if (action is not null)
            {
                action(this, FixedIds.Events.OnRemove, entity, component);
            }
        }
    }

    private void OnModified(Identifier entity, Identifier component, bool skipThisEntity = false)
    {
        var componentDefinition = GetComponentDefinition(component);
        var onModifiedActionId = GetRelationTarget(componentDefinition, (ushort)BuiltInRelationType.OnModified);
        if (onModifiedActionId == default)
        {
            return;
        }

        var action = Get<EventActionSystem>(onModifiedActionId, FixedIds.Components.EventActionSystem).Action;
        if (action is null)
        {
            return;
        }

        if (!skipThisEntity)
        {
            action(this, FixedIds.Events.OnModified, entity, component);
        }

        if (componentDefinition.HasRelation<OnExtend, Inherit>())
        {
            RecursiveOnModified(action, this, entity, component);
        }

        static void RecursiveOnModified(EventActionSystem.EventAction action, World world, Identifier entity, Identifier component)
        {
            var query = world.GetCachedQueryFor<ParamGroup<Relation<IsA, Parameter1>>>(entity);
            query.ForEach((Identifier e) =>
            {
                action(world, FixedIds.Events.OnModified, e, component);

                RecursiveOnModified(action, world, e, component);
            });
        }
    }

    private Entity CreateEntityFixedId(Identifier id)
    {
        Archetypes.Create(id);
        return new(id, this);
    }
}
