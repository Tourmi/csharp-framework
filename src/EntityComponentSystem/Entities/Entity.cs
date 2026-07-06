using static Tourmi.EntityComponentSystem.Entities.ComponentEntity;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
[DebuggerTypeProxy(typeof(EntityDebugView))]
[DebuggerDisplay("{DebugView,nq}")]
public readonly ref struct Entity(Identifier id, IEntityActions? entityActions) : IEntity<Entity>, IEquatable<Entity>, IQueryParam<Entity>
{
    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    [Obsolete("This constructor should never be used.")]
    public Entity() : this(default, default) { }

    /// <inheritdoc/>
    public Identifier Id { get; } = id;

    /// <inheritdoc cref="IEntity.Actions"/>
    internal IEntityActions? Actions { get; } = entityActions;

    /// <inheritdoc/>
    IEntityActions? IEntity.Actions => Actions;

    private EntityDebugView DebugView => new(this);

    /// <inheritdoc/>
    public static implicit operator Identifier(Entity entity) => entity.Id;

    /// <inheritdoc/>
    public static bool operator ==(Entity left, Entity right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Entity left, Entity right) => !(left == right);

    /// <inheritdoc/>
    public bool Equals(Entity other) => Id == other.Id;

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj) => false;

    static QueryParamGlobalCache IQueryParam<Entity>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParam<Entity>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static Entity IQueryParam<Entity>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex], QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache));

    [DebuggerDisplay("Id = { Id.Value }, IsAlive {IsAlive}, Name = { Name }")]
    internal class EntityDebugView(Entity entity)
    {
        private readonly Identifier _id = entity.Id;
        private readonly IEntityActions? _entityActions = entity.Actions;

        private Entity Entity => new(_id, _entityActions);

        public Identifier Id => _id;

        public bool IsAlive => Entity.IsAlive();

        public string Name => Entity.Get<Name>().Value;

        public ComponentDebugView[]? Components => ArchetypeEntry?.Archetype.Components.ToArray().Select(c => new ComponentDebugView(new(c, Entity.Actions))).ToArray();

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public (object? Value, ComponentDebugView Component)[]? ComponentValues => ArchetypeEntry?.Archetype.Components
            .ToArray()
            .Select(c => (ArchetypeEntry!.Value.GetDebugValue(c), new ComponentDebugView(new(c, Entity.Actions))))
            .ToArray();

        private ArchetypeEntityEntry? ArchetypeEntry => Entity.Actions?.World.Archetypes.GetArchetypeEntry(Entity);
    }
}
