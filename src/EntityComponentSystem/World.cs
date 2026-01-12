using System.Reflection;
using System.Runtime.CompilerServices;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Entity Component System, stores, updates, and allows for creating or querying entities.
/// </summary>
public sealed class World
{
    private readonly Dictionary<Type, Identifier> _typesToIdentifier = [];
    private readonly IdentifierCollection _ids = new();
    private readonly Identifier[] _builtInRelationIds = new Identifier[255];
    private readonly EntityArchetypeCollection _archetypes = new();
    private readonly Dictionary<Type, Query> _queryParamsToQuery = [];

    /// <summary>
    /// Region reserved for future potential optimizations.
    /// </summary>
    private readonly IdentifierRegion _reservedRegion;

    /// <summary>
    /// Special region containing identifiers for built-in relation entities.
    /// </summary>
    private readonly IdentifierRegion _builtInRelationsRegion;

    /// <summary>
    /// Region reserved for built-in components and entities.
    /// </summary>
    private readonly IdentifierRegion _coreRegion;

    /// <summary>
    /// The world's entity identifier collection.
    /// </summary>
    internal IdentifierCollection Ids => _ids;

    /// <summary>
    /// The world's archetype collection.
    /// </summary>
    internal EntityArchetypeCollection Archetypes => _archetypes;

    /// <summary>
    /// Creates and initializes a new ECS world
    /// </summary>
    private World(WorldConfiguration configuration)
    {
        _reservedRegion = new IdentifierRegion() { Offset = 0x0001, Amount = 0x00FF, Name = "Reserved" };
        _builtInRelationsRegion = new IdentifierRegion() { Offset = 0x0100, Amount = 0x0100, Name = "Built-in Relations" };
        _coreRegion = new IdentifierRegion() { Amount = 0x0400, Name = "Core" };
        _ids.Reserve(_reservedRegion);
        _ids.Reserve(_builtInRelationsRegion);
        _ids.Reserve(_coreRegion);

        foreach (var region in configuration.ReservedIdentifierRegions.EmptyIfNull())
        {
            _ids.Reserve(region);
        }

        _ids.DefaultRegionOverride = _coreRegion;
        var component = CreateEntityInternal();
        _typesToIdentifier[typeof(Component)] = component;
        _archetypes.AddComponent(component, component, null);

        var data = CreateEntityInternal();
        _typesToIdentifier[typeof(DataComponent)] = data;
        _archetypes.AddComponent(data, component, null);
        _archetypes.AddComponent(data, data, typeof(DataComponent));
        _archetypes.SetComponent(data, data, new DataComponent(typeof(DataComponent)));

        var name = CreateEntityInternal();
        _typesToIdentifier[typeof(Name)] = name;
        _archetypes.AddComponent(name, component, null);
        Set<DataComponent>(name, data, new(typeof(Name)));
        Set<Name>(name, name, new(nameof(Name)));

        Set(component, name, new Name(nameof(Component)));
        Set(data, name, new Name(nameof(DataComponent)));

        _ids.DefaultRegionOverride = _builtInRelationsRegion;
        for (var i = 0; i < _builtInRelationIds.Length; i++)
        {
            if (!Enum.IsDefined((BuiltInRelationType)(i + 1)))
            {
                continue;
            }

            var relationDefinition = CreateEntityInternal(IdentifierTypes.Relation);
            Add(relationDefinition, component);
            Set<Name>(relationDefinition, name, new(Enum.GetName((BuiltInRelationType)i + 1) ?? $"Relation #{i + 1}"));
            _builtInRelationIds[i] = relationDefinition;
        }

        // For now, configure them manually.
        _typesToIdentifier[typeof(ChildOf)] = ToId(BuiltInRelationType.ChildOf);
        _typesToIdentifier[typeof(IsA)] = ToId(BuiltInRelationType.IsA);
        _typesToIdentifier[typeof(InstanceOf)] = ToId(BuiltInRelationType.InstanceOf);
        _typesToIdentifier[typeof(Requires)] = ToId(BuiltInRelationType.Requires);
        Set(ToId(BuiltInRelationType.Requires), data, new DataComponent(typeof(Requires)));
        _typesToIdentifier[typeof(DependsOn)] = ToId(BuiltInRelationType.DependsOn);

        _ids.DefaultRegionOverride = _coreRegion;

        // todo: initialize rest of built-in components here

        _ids.DefaultRegionOverride = null;
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

    /// <summary>
    /// Creates and returns a new entity.
    /// </summary>
    public Entity CreateEntity()
    {
        var entity = new Entity(CreateEntityInternal(), this);

        return entity;
    }

    /// <summary>
    /// Returns whether the <paramref name="entity"/> is alive or not.
    /// </summary>
    public bool IsAlive(Identifier entity) => _ids.IsAlive(entity);

    /// <summary>
    /// Returns whether the <paramref name="id"/> is valid or not.
    /// If the <paramref name="id"/> is an entity, 
    ///     returns <see langword="true"/> if it is alive.
    /// If the <paramref name="id"/> is a relation between two entities,
    ///     returns <see langword="true"/> if the target entity is alive, and the relation exists.
    /// </summary>
    public bool IsValid(Identifier id)
    {
        if (!id.Types.HasFlag(IdentifierTypes.Relation))
        {
            return _ids.IsAlive(id);
        }

        var relationId = new RelationComponentIdentifier(id);
        if (relationId.RelationType == 0)
        {
            return false;
        }

        Identifier relationEntity = default;
        var relationIndex = relationId.RelationType - 1;
        if (relationIndex < _builtInRelationIds.Length)
        {
            relationEntity = _builtInRelationIds[relationIndex];
        }
        else
        {
            // TODO: Get non-built-in relations as well
        }

        return _ids.IsAlive(relationEntity) && _ids.IsAlive(relationId.Target);
    }

    /// <summary>
    /// Kills the given entity.
    /// </summary>
    public void Kill(Identifier entity)
    {
        if (_ids.IsAlive(entity))
        {
            KillEntityInternal(entity);
        }
    }

    /// <summary>
    /// Returns true if the <paramref name="entity"/> has the given <paramref name="componentId"/>.
    /// </summary>
    public bool Has(Identifier entity, Identifier componentId)
    {
        if (!_ids.IsAlive(entity) || !IsValid(componentId))
        {
            return false;
        }

        return _archetypes.HasComponent(entity, componentId);
    }

    /// <summary>
    /// Returns the <paramref name="component"/> value for the given <paramref name="entity"/>.
    /// </summary>
    public T? Get<T>(Identifier entity, Identifier component)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return default;
        }

        if (!_archetypes.HasComponent(entity, component))
        {
            return default;
        }

        return _archetypes.GetComponent<T>(entity, component);
    }

    /// <summary>
    /// Returns a mutable reference of the <paramref name="component"/> for the given <paramref name="entity"/>.
    /// </summary>
    /// <remarks>
    /// Will throw if the entity or the component are not valid.
    /// </remarks>
    public ref T? GetMutable<T>(Identifier entity, Identifier component)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        if (!_archetypes.HasComponent(entity, component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        return ref _archetypes.GetRefComponent<T>(entity, component);
    }

    /// <summary>
    /// Ensures that the <paramref name="entity"/> has the given <paramref name="component"/>,
    /// and returns its value.
    /// </summary>
    public T? Ensure<T>(Identifier entity, Identifier component)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return default;
        }

        AddComponentIfMissing(entity, component);

        return _archetypes.GetComponent<T>(entity, component);
    }

    /// <summary>
    /// Ensures that the <paramref name="entity"/> has the given <paramref name="component"/>,
    /// and returns a reference to its value.
    /// </summary>
    public ref T? EnsureMutable<T>(Identifier entity, Identifier component)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        AddComponentIfMissing(entity, component);

        return ref _archetypes.GetRefComponent<T>(entity, component);
    }

    /// <summary>
    /// Adds the given <paramref name="component"/> to the <paramref name="entity"/>, without any associated data.
    /// </summary>
    public void Add(Identifier entity, Identifier component)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        AddComponentIfMissing(entity, component);
    }

    /// <summary>
    /// Sets the <paramref name="component"/>'s value for the <paramref name="entity"/> to the given <paramref name="value"/>
    /// </summary>
    public void Set<T>(Identifier entity, Identifier component, T value)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        AddComponentIfMissing(entity, component);

        _archetypes.SetComponent(entity, component, value);
    }

    /// <summary>
    /// Removes the given <paramref name="component"/> from the <paramref name="entity"/>.
    /// </summary>
    public void Remove(Identifier entity, Identifier component)
    {
        if (!_ids.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        if (!_archetypes.HasComponent(entity, component))
        {
            return;
        }

        _archetypes.RemoveComponent(entity, component);
    }

    /// <summary>
    /// Returns the entity representing the component of the given <typeparamref name="TComponent"/> type.
    /// </summary>
    public ComponentEntity GetComponentForType<TComponent>() => GetComponentForType(typeof(TComponent));

    /// <summary>
    /// Returns the entity representing the component of the given <paramref name="type"/>.
    /// </summary>
    public ComponentEntity GetComponentForType(Type type)
    {
        if (!_typesToIdentifier.TryGetValue(type, out var componentId) || !IsAlive(componentId))
        {
            var entity = CreateEntity();
            entity.Add<Component>();
            entity.Set(new Name(type.Name));

            if (type.GetCustomAttribute<TagComponentAttribute>() is null)
            {
                entity.Set(new DataComponent(type));
            }

            if (type.GetCustomAttribute<ComponentRelationTypeAttribute>() is not null)
            {
                entity.Add<RelationDefinition>();
            }

            componentId = entity.Id;
            _typesToIdentifier[type] = componentId;
        }

        return new(componentId, this);
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

    private void AddComponentIfMissing(Identifier entity, Identifier component)
    {
        if (_archetypes.HasComponent(entity, component))
        {
            return;
        }

        var datatypeId = component;
        if (component.Types.HasFlag(IdentifierTypes.Relation))
        {
            var relationId = new RelationComponentIdentifier(component);
            if (relationId.BuiltInRelationType != BuiltInRelationType.None)
            {
                datatypeId = ToId(relationId.BuiltInRelationType);
            }
            else
            {
                // TODO: Also process user-created relations.
            }
        }

        var dataType = Get<DataComponent>(datatypeId, GetComponentForType<DataComponent>());
        _archetypes.AddComponent(entity, component, dataType.DataType);
    }

    private Identifier CreateEntityInternal(IdentifierTypes identifierTypes = IdentifierTypes.None)
    {
        var id = _ids.Create(identifierTypes);
        _archetypes.Create(id);
        return id;
    }

    private void KillEntityInternal(Identifier id)
    {
        _archetypes.Kill(id);
        _ids.Free(id);
    }

    private Identifier ToId(BuiltInRelationType relationType) => _builtInRelationIds[(int)relationType - 1];
}
