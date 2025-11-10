using System.Reflection;
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
        AddComponent(component, component);
        _typesToIdentifier[typeof(Component)] = component;

        var data = _entities.Create(_builtInRegion);
        AddComponent(data, component);
        SetComponent(data, data, typeof(Type));
        _typesToIdentifier[typeof(DataComponent)] = data;

        var name = _entities.Create(_builtInRegion);
        AddComponent(name, component);
        SetComponent(name, data, typeof(string));
        SetComponent(name, name, nameof(Name));
        _typesToIdentifier[typeof(Name)] = name;

        SetComponent(component, name, nameof(Component));
        SetComponent(data, name, nameof(DataComponent));

        // todo: initialize built-in components
    }

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
    public bool IsAlive(Identifier identifier) => _entities.IsAlive(identifier);

    /// <summary>
    /// Kills the given entity.
    /// </summary>
    public void Kill(Identifier identifier) => _entities.Kill(identifier);

    /// <summary>
    /// Returns the component of the given <typeparamref name="TComponent"/> type.
    /// </summary>
    public ComponentEntity GetComponent<TComponent>() => GetOrRegisterComponentEntity<TComponent>();

    /// <summary>
    /// Adds the given <paramref name="component"/> to the <paramref name="targetEntity"/>, without any associated data.
    /// </summary>
    public void AddComponent(Identifier targetEntity, Identifier component)
    {
        if (!_entities.IsAlive(targetEntity) || !_entities.IsAlive(component))
        {
            return;
        }

    }

    /// <summary>
    /// Sets the <paramref name="component"/>'s value for the <paramref name="targetEntity"/> to the given <paramref name="value"/>
    /// </summary>
    public void SetComponent<T>(Identifier targetEntity, Identifier component, T value)
    {
        if (!_entities.IsAlive(targetEntity) || !_entities.IsAlive(component))
        {
            return;
        }

    }

    private ComponentEntity GetOrRegisterComponentEntity<TComponent>()
    {
        if (!_typesToIdentifier.TryGetValue(typeof(TComponent), out var componentId))
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
