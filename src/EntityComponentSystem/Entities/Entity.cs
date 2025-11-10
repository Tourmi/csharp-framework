using System.Diagnostics.CodeAnalysis;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Entity that exists in an ecs <see cref="EntityComponentSystem.World" /> instance.
/// </summary>
public readonly struct Entity : IEquatable<Entity>
{
    /// <summary>
    /// Identifier of the entity
    /// </summary>
    public Identifier Id { get; }

    internal World? World { get; }

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
}
