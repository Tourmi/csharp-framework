namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Hinted entity representing a prefab that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
public readonly ref struct PrefabEntity(Identifier id, IEntityActions? entityActions) : IEntity<PrefabEntity>, IQueryParam<PrefabEntity>
{
    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    [Obsolete("This constructor should never be used.")]
    public PrefabEntity() : this(default, default) { }

    internal PrefabEntity(Entity entity) : this(entity.Id, entity.Actions) { }

    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    /// <inheritdoc cref="IEntity.Actions"/>
    internal IEntityActions? Actions { get; } = entityActions;

    /// <inheritdoc/>
    IEntityActions? IEntity.Actions => Actions;

    /// <inheritdoc/>
    public static implicit operator Identifier(PrefabEntity entity) => entity.Id;

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(PrefabEntity entity) => new(entity.Id, entity.Actions);

    /// <summary>
    /// Explicitely casts the entity to a prefab entity.
    /// </summary>
    public static explicit operator PrefabEntity(Entity entity) => new(entity);

    static PrefabEntity IQueryParam<PrefabEntity>.CreateFrom(QueryParamEntityInfo info)
        => new(info.Archetype.Entities[info.EntityIndex], info.World);

    static void IQueryParam<PrefabEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<Prefab>();
}
