using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
[DebuggerTypeProxy(typeof(EntityDebugView))]
[DebuggerDisplay("{DebugView,nq}")]
public readonly struct Entity : IEquatable<Entity>
{
    /// <summary>
    /// Identifier of the entity
    /// </summary>
    public Identifier Id { get; }

    internal World? World { get; }

    private EntityDebugView DebugView => new(this);

    internal Entity(Identifier id, World? world)
    {
        Id = id;
        World = world;
    }

    /// <summary>
    /// Returns the identifier of the entity implicitely
    /// </summary>
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
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Entity other && Equals(other);

    [DebuggerDisplay("Id = { Id.Value }, IsAlive {IsAlive}, Name = { Name }")]
    internal class EntityDebugView(Entity entity)
    {
        private readonly Entity _entity = entity;

        public Identifier Id => _entity.Id;

        public bool IsAlive => _entity.IsAlive();

        public string Name => _entity.Get<Name>().Value;

        public ComponentEntity[]? Components => ArchetypeEntry?.Archetype.Components.Select(c => new ComponentEntity(c, _entity.World)).ToArray();

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public (object? Value, ComponentEntity Component)[]? ComponentValues => ArchetypeEntry?.Archetype.Components
            .Select(c => (ArchetypeEntry!.Value.GetDebugValue(c), new ComponentEntity(c, _entity.World)))
            .ToArray();

        private ArchetypeEntityEntry? ArchetypeEntry => _entity.World?.Archetypes.GetArchetypeEntry(_entity);
    }
}
