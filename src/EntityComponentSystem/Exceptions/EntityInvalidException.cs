namespace Tourmi.EntityComponentSystem.Exceptions;

/// <summary>
/// Exception thrown when an operation cannot be completed due to an entity being invalid or dead.
/// </summary>
public class EntityInvalidException(Identifier entityId, string? message, Exception? innerException) : InvalidOperationException(message, innerException)
{
    /// <summary>
    /// Id of the relevant Entity.
    /// </summary>
    public Identifier EntityId { get; } = entityId;

    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException(string? message, Exception? innerException) : this(default, message, innerException) { }

    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException(string? message) : this(default, message, null) { }

    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException() : this(default, "Cannot complete operation due to entity being invalid.", null) { }

    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException(Identifier entityId, string? message) : this(entityId, message, null) { }

    /// <inheritdoc cref="EntityInvalidException"/>
    public EntityInvalidException(Identifier entityId) : this(entityId, $"Cannot complete operation due to entity with id {entityId} being invalid.", null) { }

    [DoesNotReturn]
    internal static void ThrowEntityUninitialized(Identifier id) => throw new EntityInvalidException(id, $"Entity {id} is uninitialized!");

    [DoesNotReturn]
    internal static void ThrowEntityInvalid(Identifier id) => throw new EntityInvalidException(id, $"Entity {id} is invalid or dead!");

    [DoesNotReturn]
    internal static void ThrowComponentInvalid(Identifier id) => throw new EntityInvalidException(id, $"Component {id} is invalid!");

    [DoesNotReturn]
    internal static void ThrowMissingComponent(Identifier entityId, Identifier componentId) => throw new EntityInvalidException(entityId, $"Entity {entityId} does not have component {componentId}!");
}
