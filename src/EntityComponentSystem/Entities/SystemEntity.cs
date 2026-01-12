namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// An entity representing a system.
/// </summary>
public readonly ref struct SystemEntity(Identifier id, World? world) : IEntity<SystemEntity>, IQueryParam<SystemEntity>
{
    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    internal World? World { get; } = world;

    /// <inheritdoc/>
    World? IEntity.World => World;

    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    public SystemEntity() : this(default, default) { }

    internal SystemEntity(Entity entity) : this(entity.Id, entity.World) { }

    /// <inheritdoc/>
    public static implicit operator Identifier(SystemEntity entity) => entity.Id;

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(SystemEntity entity) => new(entity.Id, entity.World);

    /// <summary>
    /// Explicitely casts the entity to a system entity.
    /// </summary>
    public static explicit operator SystemEntity(Entity entity) => new(entity);

    static SystemEntity IQueryParam<SystemEntity>.CreateFrom(QueryParamEntityInfo info)
        => new(info.Archetype.Entities[info.EntityIndex], info.World);

    static void IQueryParam<SystemEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<Components.SystemComponent>();
}
