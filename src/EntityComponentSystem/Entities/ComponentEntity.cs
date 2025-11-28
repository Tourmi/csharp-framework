using System.Diagnostics;
using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity representing a component that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
[DebuggerTypeProxy(typeof(ComponentDebugView))]
[DebuggerDisplay("{DebugView,nq}")]
public readonly struct ComponentEntity(Identifier id, World? world)
{
    /// <inheritdoc cref="Entity.Id"/>
    public Identifier Id { get; } = id;

    internal World? World { get; } = world;

    private ComponentDebugView DebugView => new(this);

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

    [DebuggerDisplay("Id = { Id.Value }, Name = { Name }, DataType = { DataType }")]
    internal class ComponentDebugView(ComponentEntity entity)
    {
        private readonly Entity _entity = entity;

        public Identifier Id => _entity.Id;

        public string Name => _entity.Get<Name>().Value;

        public Type? DataType => _entity.Get<DataComponent>().DataType;

        public ComponentEntity[]? Components => ArchetypeEntry?.Archetype?.Components.Select(c => new ComponentEntity(c, _entity.World)).ToArray();

        public string[]? ComponentValues => ArchetypeEntry?.Archetype.Components
            .Select(c => (Component: new ComponentEntity(c, _entity.World), Value: ArchetypeEntry!.Value.GetDebugValue(c)))
            .Select(t => $"Component: {t.Component}, Value: {t.Value ?? "NULL"}")
            .ToArray();

        private ArchetypeEntityEntry? ArchetypeEntry => _entity.World?.Entities.GetArchetypeEntry(_entity);
    }
}
