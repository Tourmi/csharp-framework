using System.Diagnostics.CodeAnalysis;

namespace Tourmi.EntityComponentSystem.Exceptions;

/// <summary>
/// Exception thrown when an operation cannot be completed due to an entity being invalid or dead.
/// </summary>
public class EntityInvalidException(string? message, Exception? innerException) : InvalidOperationException(message, innerException)
{
    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException(string? message) : this(message, null) { }

    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException() : this("Cannot complete operation due to entity being dead or invalid.", null) { }

    [DoesNotReturn]
    internal static void ThrowEntityUninitialized(Identifier id) => throw new EntityInvalidException($"Entity {id} is uninitialized!");

    [DoesNotReturn]
    internal static void ThrowEntityInvalid(Identifier id) => throw new EntityInvalidException($"Entity {id} is invalid or dead!");

    [DoesNotReturn]
    internal static void ThrowComponentInvalid(Identifier id) => throw new EntityInvalidException($"Component {id} is invalid!");

    [DoesNotReturn]
    internal static void ThrowMissingComponent(Identifier entityId, Identifier componentId) => throw new EntityInvalidException($"Entity {entityId} does not have component {componentId}!");
}
