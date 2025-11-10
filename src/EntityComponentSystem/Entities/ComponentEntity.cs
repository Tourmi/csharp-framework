namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity representing a component that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
public readonly struct ComponentEntity(Identifier id, World? world)
{
    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    internal World? World { get; } = world;

    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    public ComponentEntity() : this(default, default) { }

    internal ComponentEntity(Entity entity) : this(entity.Id, entity.World) { }

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(ComponentEntity entity) => new(entity.Id, entity.World);

    /// <summary>
    /// Returns the identifier of the entity implicitely
    /// </summary>
    public static implicit operator Identifier(ComponentEntity entity) => entity.Id;

    /// <summary>
    /// Explicitely casts the entity to a component entity.
    /// </summary>
    public static explicit operator ComponentEntity(Entity entity) => new(entity);
}
