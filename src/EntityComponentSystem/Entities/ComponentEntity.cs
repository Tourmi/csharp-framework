using System.Diagnostics;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Components.Metacomponents;
using Tourmi.EntityComponentSystem.Queries;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity representing a component that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
[DebuggerTypeProxy(typeof(ComponentDebugView))]
[DebuggerDisplay("{DebugView,nq}")]
public readonly ref struct ComponentEntity(Identifier id, World? world) : IEntity<ComponentEntity>, IQueryParam<ComponentEntity>
{
    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    /// <inheritdoc/>
    internal World? World { get; } = world;

    /// <inheritdoc/>
    World? IEntity.World => World;

    private ComponentDebugView DebugView => new(this);

    /// <summary>
    /// Constructs an invalid entity.
    /// </summary>
    public ComponentEntity() : this(default, default) { }

    internal ComponentEntity(Entity entity) : this(entity.Id, entity.World) { }

    /// <inheritdoc/>
    public static implicit operator Identifier(ComponentEntity entity) => entity.Id;

    /// <summary>
    /// Returns the non-hinted entity implicitely
    /// </summary>
    public static implicit operator Entity(ComponentEntity entity) => new(entity.Id, entity.World);

    /// <summary>
    /// Explicitely casts the entity to a component entity.
    /// </summary>
    public static explicit operator ComponentEntity(Entity entity) => new(entity.Id, entity.World);

    static ComponentEntity IQueryParam<ComponentEntity>.CreateFrom(QueryParamEntityInfo info)
        => new(info.Archetype.Entities[info.EntityIndex], info.World);

    static void IQueryParam<ComponentEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<Component>();

    [DebuggerDisplay("Id = { Id.Value }, Name = { Name }, DataType = { DataType }")]
    internal class ComponentDebugView(ComponentEntity entity)
    {
        private readonly Identifier _id = entity.Id;
        private readonly World? _world = entity.World;

        private Entity Entity => new(_id, _world);

        public Identifier Id => _id;

        public string Name => Entity.Get<Name>().Value;

        public Type? DataType => Entity.Get<DataComponent>().DataType;

        public ComponentDebugView[]? Components => ArchetypeEntry?.Archetype?.Components.ToArray()
            .Select(c => new ComponentDebugView(new(c, Entity.World)))
            .ToArray();

        public string[]? ComponentValues => ArchetypeEntry?.Archetype.Components.ToArray()
            .Select(c => (Component: new ComponentDebugView(new(c, Entity.World)), Value: ArchetypeEntry!.Value.GetDebugValue(c)))
            .Select(t => $"Component: {t.Component}, Value: {t.Value ?? "NULL"}")
            .ToArray();

        private ArchetypeEntityEntry? ArchetypeEntry => Entity.World?.Archetypes.GetArchetypeEntry(Entity);
    }
}
