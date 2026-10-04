using System.Diagnostics;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity representing a component that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
[DebuggerTypeProxy(typeof(ComponentDebugView))]
[DebuggerDisplay("{DebugView,nq}")]
public readonly ref struct ComponentEntity(Identifier id, IEntityActions? world) : IEntity<ComponentEntity>, IQueryParam<ComponentEntity>
{
    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    [Obsolete("This constructor should never be used.")]
    public ComponentEntity() : this(default, default) { }

    internal ComponentEntity(Entity entity) : this(entity.Id, entity.Actions) { }

    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    /// <inheritdoc cref="IEntity.Actions"/>
    internal IEntityActions? Actions { get; } = world;

    /// <inheritdoc/>
    IEntityActions? IEntity.Actions => Actions;

    private ComponentDebugView DebugView => new(this);

    /// <inheritdoc/>
    public static implicit operator Identifier(ComponentEntity entity) => entity.Id;

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(ComponentEntity entity) => new(entity.Id, entity.Actions);

    /// <summary>
    /// Explicitely casts the entity to a component entity.
    /// </summary>
    public static explicit operator ComponentEntity(Entity entity) => new(entity.Id, entity.Actions);

    static QueryParamGlobalCache IQueryParam<ComponentEntity>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParam<ComponentEntity>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static ComponentEntity IQueryParam<ComponentEntity>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex], QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache));

    static void IQueryParam<ComponentEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<Component>();

    [DebuggerDisplay("Id = { Id.Value }, Name = { Name }, DataType = { DataType }")]
    internal class ComponentDebugView(ComponentEntity entity)
    {
        private readonly Identifier _id = entity.Id;
        private readonly IEntityActions? _entityActions = entity.Actions;

        private Entity Entity => new(_id, _entityActions);

        public Identifier Id => _id;

        public string Name => Entity.Get<Name>().Value;

        public Type? DataType => Entity.Get<DataComponent>().DataType;

        public ComponentDebugView[]? Components => ArchetypeEntry?.Archetype?.Components.ToArray()
            .Select(c => new ComponentDebugView(new(c, Entity.Actions)))
            .ToArray();

        public string[]? ComponentValues => ArchetypeEntry?.Archetype.Components.ToArray()
            .Select(c => (Component: new ComponentDebugView(new(c, Entity.Actions)), Value: ArchetypeEntry!.Value.GetDebugValue(c)))
            .Select(t => $"Component: {t.Component}, Value: {t.Value ?? "NULL"}")
            .ToArray();

        private ArchetypeEntityEntry? ArchetypeEntry => Entity.Actions?.World.Archetypes.GetArchetypeEntry(Entity);
    }
}
