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
    private readonly EntityIdentifierCollection _entities;

    /// <summary>
    /// Region reserved for future potential optimizations.
    /// </summary>
    private readonly IdentifierRegion _reservedRegion;

    /// <summary>
    /// Region reserved for built-in components and entities.
    /// </summary>
    private readonly IdentifierRegion _builtInRegion;

    /// <summary>
    /// Creates and initializes a new ECS
    /// </summary>
    private World(WorldConfiguration configuration)
    {
        _entities = new(_emptyArchetype);

        _reservedRegion = new IdentifierRegion() { Offset = 0x0001, Amount = 0x0100 - 1, Name = "Reserved" };
        _builtInRegion = new IdentifierRegion() { Offset = 0x0100, Amount = 0x0400, Name = "BuiltIn" };
        _entities.Reserve(_reservedRegion);
        _entities.Reserve(_builtInRegion);

        foreach (var region in configuration.ReservedIdentifierRegions.EmptyIfNull())
        {
            _entities.Reserve(region);
        }

        var component = _entities.Create(_builtInRegion);
        _typesToIdentifier[typeof(Component)] = component;
        _entities.AddComponent(component, component, null);

        var data = _entities.Create(_builtInRegion);
        _typesToIdentifier[typeof(DataComponent)] = data;
        _entities.AddComponent(data, component, null);
        _entities.AddComponent(data, data, typeof(DataComponent));
        _entities.SetComponent(data, data, new DataComponent() { DataType = typeof(DataComponent) });

        var name = _entities.Create(_builtInRegion);
        _typesToIdentifier[typeof(Name)] = name;
        _entities.AddComponent(name, component, null);
        Set<DataComponent>(name, data, new(typeof(Name)));
        Set<Name>(name, name, new(nameof(Name)));

        Set(component, name, new Name(nameof(Component)));
        Set(data, name, new Name(nameof(DataComponent)));

        // todo: initialize rest of built-in components
    }

    /// <summary>
    /// Collection used for testing or debugging purposes.
    /// </summary>
    internal EntityIdentifierCollection Entities => _entities;

    /// <summary>
    /// Creates a new 
    /// </summary>
    /// <returns></returns>
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
    /// Returns whether the given entity is alive or not.
    /// </summary>
    public bool IsAlive(Identifier entity) => _entities.IsAlive(entity);

    /// <summary>
    /// Kills the given entity.
    /// </summary>
    public void Kill(Identifier entity) => _entities.Kill(entity);

    /// <summary>
    /// Returns true if the <paramref name="targetEntity"/> has the given <paramref name="component"/>.
    /// </summary>
    public bool Has(Identifier targetEntity, Identifier component)
    {
        if (!_entities.IsAlive(targetEntity) || !_entities.IsAlive(component))
        {
            return false;
        }

        return _entities.HasComponent(targetEntity, component);
    }

    /// <summary>
    /// Returns the <paramref name="component"/> value for the given <paramref name="entity"/>.
    /// </summary>
    public T? Get<T>(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !_entities.IsAlive(component))
        {
            return default!;
        }

        return _entities.GetComponent<T>(entity, component);
    }

    /// <summary>
    /// Returns a mutable reference of the <paramref name="component"/> for the given <paramref name="entity"/>.
    /// </summary>
    public ref T? GetMutable<T>(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !_entities.IsAlive(component))
        {
            return ref StrongBox<T?>.Default.Value;
        }

        return ref _entities.GetRefComponent<T>(entity, component);
    }

    /// <summary>
    /// Adds the given <paramref name="component"/> to the <paramref name="targetEntity"/>, without any associated data.
    /// </summary>
    public void Add(Identifier targetEntity, Identifier component)
    {
        if (!_entities.IsAlive(targetEntity) || !_entities.IsAlive(component))
        {
            return;
        }

        var dataType = Get<DataComponent>(component, GetComponentForType<DataComponent>());

        _entities.AddComponent(targetEntity, component, dataType.DataType);
    }

    /// <summary>
    /// Sets the <paramref name="component"/>'s value for the <paramref name="targetEntity"/> to the given <paramref name="value"/>
    /// </summary>
    public void Set<T>(Identifier targetEntity, Identifier component, T value)
    {
        if (!_entities.IsAlive(targetEntity) || !_entities.IsAlive(component))
        {
            return;
        }

        if (!Has(targetEntity, component))
        {
            Add(targetEntity, component);
        }

        _entities.SetComponent(targetEntity, component, value);
    }

    /// <summary>
    /// Removes the given <paramref name="component"/> from the <paramref name="entity"/>.
    /// </summary>
    public void Remove(Identifier entity, Identifier component)
    {
        if (!_entities.IsAlive(entity) || !_entities.IsAlive(component))
        {
            return;
        }

        if (!Has(entity, component))
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

            if (typeof(TComponent).GetCustomAttribute<FlagComponentAttribute>() is null && typeof(TComponent).GetCustomAttribute<RelationFlagAttribute>() is null)
            {
                entity.Set(new DataComponent() { DataType = typeof(TComponent) });
            }

            componentId = entity.Id;
            _typesToIdentifier[typeof(TComponent)] = componentId;
        }

        return new(componentId, this);
    }
}
