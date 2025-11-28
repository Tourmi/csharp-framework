using System.Reflection;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Attributes;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Entity Component System, stores, updates, and allows for creating or querying entities.
/// </summary>
public class World
{
    private readonly Archetype _emptyArchetype = Archetype.Create();
    private readonly Dictionary<Type, Identifier> _typesToIdentifier = [];
    private readonly IdentifierCollection _entities;
    private readonly Identifier[] _builtInRelationIdentifiers = new Identifier[255];

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
    /// Collection used for testing or debugging purposes.
    /// </summary>
    internal IdentifierCollection Entities => _entities;

    /// <summary>
    /// Creates and initializes a new ECS world
    /// </summary>
    private World(WorldConfiguration configuration)
    {
        _entities = new(_emptyArchetype);

        _reservedRegion = new IdentifierRegion() { Offset = 0x0001, Amount = 0x00FF, Name = "Reserved" };
        _builtInRelationsRegion = new IdentifierRegion() { Offset = 0x0100, Amount = 0x0100, Name = "Built-in Relations" };
        _coreRegion = new IdentifierRegion() { Amount = 0x0400, Name = "Core" };
        _entities.Reserve(_reservedRegion);
        _entities.Reserve(_builtInRelationsRegion);
        _entities.Reserve(_coreRegion);

        foreach (var region in configuration.ReservedIdentifierRegions.EmptyIfNull())
        {
            _entities.Reserve(region);
        }

        _entities.DefaultRegionOverride = _coreRegion;
        var component = _entities.Create();
        _typesToIdentifier[typeof(Component)] = component;
        _entities.AddComponent(component, component, null);

        var data = _entities.Create();
        _typesToIdentifier[typeof(DataComponent)] = data;
        _entities.AddComponent(data, component, null);
        _entities.AddComponent(data, data, typeof(DataComponent));
        _entities.SetComponent(data, data, new DataComponent(typeof(DataComponent)));

        var name = _entities.Create();
        _typesToIdentifier[typeof(Name)] = name;
        _entities.AddComponent(name, component, null);
        Set<DataComponent>(name, data, new(typeof(Name)));
        Set<Name>(name, name, new(nameof(Name)));

        Set(component, name, new Name(nameof(Component)));
        Set(data, name, new Name(nameof(DataComponent)));

        _entities.DefaultRegionOverride = _builtInRelationsRegion;
        for (var i = 0; i < _builtInRelationIdentifiers.Length; i++)
        {
            if (!Enum.IsDefined((BuiltInRelationType)(i + 1)))
            {
                break;
            }

            var relationDefinition = _entities.Create(IdentifierTypes.Relation);
            Add(relationDefinition, component);
            Set<Name>(relationDefinition, name, new(Enum.GetName((BuiltInRelationType)i + 1) ?? $"Relation #{i + 1}"));
            _builtInRelationIdentifiers[i] = relationDefinition;
        }

        // For now, configure them manually.
        _typesToIdentifier[typeof(ChildOf)] = ToId(BuiltInRelationType.ChildOf);
        _typesToIdentifier[typeof(IsA)] = ToId(BuiltInRelationType.IsA);
        _typesToIdentifier[typeof(InstanceOf)] = ToId(BuiltInRelationType.InstanceOf);
        _typesToIdentifier[typeof(DependsOn)] = ToId(BuiltInRelationType.DependsOn);
        Set(ToId(BuiltInRelationType.DependsOn), data, new DataComponent(typeof(DependsOn)));

        _entities.DefaultRegionOverride = _coreRegion;

        // todo: initialize rest of built-in components here

        _entities.DefaultRegionOverride = null;
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
        var entity = new Entity(_entities.Create(), this);

        return entity;
    }

    /// <summary>
    /// Returns whether the <paramref name="entity"/> is alive or not.
    /// </summary>
    public bool IsAlive(Identifier entity) => _entities.IsAlive(entity);

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
            return _entities.IsAlive(id);
        }

        var relationId = new RelationComponentIdentifier(id);
        if (relationId.RelationType == 0)
        {
            return false;
        }

        Identifier relationEntity = default;
        var relationIndex = relationId.RelationType - 1;
        if (relationIndex < _builtInRelationIdentifiers.Length)
        {
            relationEntity = _builtInRelationIdentifiers[relationIndex];
        }
        else
        {
            // TODO: Get non-built-in relations as well
        }

        return _entities.IsAlive(relationEntity) && _entities.IsAlive(relationId.Target);
    }

    /// <summary>
    /// Kills the given entity.
    /// </summary>
    public void Kill(Identifier entity)
    {
        if (_entities.IsAlive(entity))
        {
            _entities.Free(entity);
        }
    }

    /// <summary>
    /// Returns true if the <paramref name="entity"/> has the given <paramref name="componentId"/>.
    /// </summary>
    public bool Has(Identifier entity, Identifier componentId)
    {
        if (!_entities.IsAlive(entity) || !IsValid(componentId))
        {
            return false;
        }

        return _entities.HasComponent(entity, componentId);
    }

    /// <summary>
    /// Returns the <paramref name="component"/> value for the given <paramref name="entity"/>.
    /// </summary>
    public T? Get<T>(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !IsValid(component))
        {
            return default;
        }

        if (!_entities.HasComponent(entity, component))
        {
            return default;
        }

        return _entities.GetComponent<T>(entity, component);
    }

    /// <summary>
    /// Returns a mutable reference of the <paramref name="component"/> for the given <paramref name="entity"/>.
    /// </summary>
    /// <remarks>
    /// Will throw if the entity or the component are not valid.
    /// </remarks>
    public ref T? GetMutable<T>(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !IsValid(component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        if (!_entities.HasComponent(entity, component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        return ref _entities.GetRefComponent<T>(entity, component);
    }

    /// <summary>
    /// Ensures that the <paramref name="entity"/> has the given <paramref name="component"/>,
    /// and returns its value.
    /// </summary>
    public T? Ensure<T>(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !IsValid(component))
        {
            return default;
        }

        AddComponentIfMissing(entity, component);

        return _entities.GetComponent<T>(entity, component);
    }

    /// <summary>
    /// Ensures that the <paramref name="entity"/> has the given <paramref name="component"/>,
    /// and returns a reference to its value.
    /// </summary>
    public ref T? EnsureMutable<T>(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !IsValid(component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        AddComponentIfMissing(entity, component);

        return ref _entities.GetRefComponent<T>(entity, component);
    }

    /// <summary>
    /// Adds the given <paramref name="component"/> to the <paramref name="entity"/>, without any associated data.
    /// </summary>
    public void Add(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !IsValid(component))
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
        if (!_entities.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        AddComponentIfMissing(entity, component);

        _entities.SetComponent(entity, component, value);
    }

    /// <summary>
    /// Removes the given <paramref name="component"/> from the <paramref name="entity"/>.
    /// </summary>
    public void Remove(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !IsValid(component))
        {
            return;
        }

        if (!_entities.HasComponent(entity, component))
        {
            return;
        }

        _entities.RemoveComponent(entity, component);
    }

    /// <summary>
    /// Returns the entity representing the component of the given <typeparamref name="TComponent"/> type.
    /// </summary>
    public ComponentEntity GetComponentForType<TComponent>()
    {
        if (!_typesToIdentifier.TryGetValue(typeof(TComponent), out var componentId) || !IsAlive(componentId))
        {
            var entity = CreateEntity();
            entity.Add<Component>();
            entity.Set(new Name(typeof(TComponent).Name));

            if (typeof(TComponent).GetCustomAttribute<ComponentFlagAttribute>() is null)
            {
                entity.Set(new DataComponent(typeof(TComponent)));
            }

            if (typeof(TComponent).GetCustomAttribute<ComponentRelationTypeAttribute>() is not null)
            {
                entity.Add<RelationDefinition>();
            }

            componentId = entity.Id;
            _typesToIdentifier[typeof(TComponent)] = componentId;
        }

        return new(componentId, this);
    }

    private void AddComponentIfMissing(Identifier entity, Identifier component)
    {
        if (_entities.HasComponent(entity, component))
        {
            return;
        }

        var datatypeId = component;
        if (component.Types.HasFlag(IdentifierTypes.Relation))
        {
            var relationId = new RelationComponentIdentifier(component);
            var builtInRelationType = relationId.BuiltInRelationType;
            if (builtInRelationType != BuiltInRelationType.None)
            {
                datatypeId = ToId(builtInRelationType);
            }
            else
            {
                // TODO: Also deal with user-created relations.
            }
        }

        var dataType = Get<DataComponent>(datatypeId, GetComponentForType<DataComponent>());
        _entities.AddComponent(entity, component, dataType.DataType);
    }

    private Identifier ToId(BuiltInRelationType relationType) => _builtInRelationIdentifiers[(int)relationType - 1];
}
