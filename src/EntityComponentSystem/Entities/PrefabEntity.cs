namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Hinted entity representing a prefab that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
public readonly ref struct PrefabEntity(Identifier id, World? world) : IEntity<PrefabEntity>
{
    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    internal World? World { get; } = world;

    /// <inheritdoc/>
    World? IEntity.World => World;

    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    public PrefabEntity() : this(default, default) { }

    internal PrefabEntity(Entity entity) : this(entity.Id, entity.World) { }

    /// <inheritdoc/>
    public static implicit operator Identifier(PrefabEntity entity) => entity.Id;

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(PrefabEntity entity) => new(entity.Id, entity.World);

    /// <summary>
    /// Explicitely casts the entity to a prefab entity.
    /// </summary>
    public static explicit operator PrefabEntity(Entity entity) => new(entity);
}
