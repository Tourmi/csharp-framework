namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// An entity representing a system.
/// </summary>
public readonly ref struct SystemEntity(Identifier id, IEntityActions? entityActions) : IEntity<SystemEntity>, IQueryParam<SystemEntity>
{
    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    [Obsolete("This constructor should never be used.")]
    public SystemEntity() : this(default, default) { }

    internal SystemEntity(Entity entity) : this(entity.Id, entity.Actions) { }

    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    /// <inheritdoc cref="IEntity.Actions"/>
    internal IEntityActions? Actions { get; } = entityActions;

    /// <inheritdoc/>
    IEntityActions? IEntity.Actions => Actions;

    /// <inheritdoc/>
    public static implicit operator Identifier(SystemEntity entity) => entity.Id;

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(SystemEntity entity) => new(entity.Id, entity.Actions);

    /// <summary>
    /// Explicitely casts the entity to a system entity.
    /// </summary>
    public static explicit operator SystemEntity(Entity entity) => new(entity);

    static SystemEntity IQueryParam<SystemEntity>.CreateFrom(QueryParamEntityInfo info)
        => new(info.Archetype.Entities[info.EntityIndex], info.World);

    static void IQueryParam<SystemEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<SystemComponent>();
}
