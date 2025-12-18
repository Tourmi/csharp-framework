using System.Diagnostics;
using Tourmi.EntityComponentSystem.Archetypes;
using static Tourmi.EntityComponentSystem.Entities.ComponentEntity;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
[DebuggerTypeProxy(typeof(EntityDebugView))]
[DebuggerDisplay("{DebugView,nq}")]
public readonly ref struct Entity(Identifier id, World? world) : IEntity<Entity>, IEquatable<Entity>
{
    /// <inheritdoc/>
    public Identifier Id { get; } = id;

    internal World? World { get; } = world;

    /// <inheritdoc/>
    World? IEntity.World => World;

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

    [DebuggerDisplay("Id = { Id.Value }, IsAlive {IsAlive}, Name = { Name }")]
    internal class EntityDebugView(Entity entity)
    {
        private readonly Identifier _id = entity.Id;
        private readonly World? _world = entity.World;

        private Entity Entity => new(_id, _world);

        public Identifier Id => _id;

        public bool IsAlive => Entity.IsAlive();

        public string Name => Entity.Get<Name>().Value;

        public ComponentDebugView[]? Components => ArchetypeEntry?.Archetype.Components.Select(c => new ComponentDebugView(new(c, Entity.World))).ToArray();

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public (object? Value, ComponentDebugView Component)[]? ComponentValues => ArchetypeEntry?.Archetype.Components
            .Select(c => (ArchetypeEntry!.Value.GetDebugValue(c), new ComponentDebugView(new(c, Entity.World))))
            .ToArray();

        private ArchetypeEntityEntry? ArchetypeEntry => Entity.World?.Archetypes.GetArchetypeEntry(Entity);
    }
}
