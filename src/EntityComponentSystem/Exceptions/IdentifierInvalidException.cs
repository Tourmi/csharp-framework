namespace Tourmi.EntityComponentSystem.Exceptions;

/// <summary>
/// Exception thrown when an operation cannot be completed due to an entity being invalid or dead.
/// </summary>
public class IdentifierInvalidException(Identifier id, string? message, Exception? innerException) : InvalidOperationException(message, innerException)
{
    /// <summary>
    /// Identifier that is invalid.
    /// </summary>
    public Identifier Identifier { get; } = id;

    /// <inheritdoc cref="IdentifierInvalidException"/>
    public IdentifierInvalidException(string? message, Exception? innerException) : this(default, message, null) { }

    /// <inheritdoc cref="IdentifierInvalidException"/>
    public IdentifierInvalidException(string? message) : this(message, null) { }

    /// <inheritdoc cref="IdentifierInvalidException"/>
    public IdentifierInvalidException(Identifier id, string? message) : this(id, message, null) { }

    /// <inheritdoc cref="IdentifierInvalidException"/>
    public IdentifierInvalidException(Identifier id) : this(id, $"Cannot complete operation due to the given identifier {id} being invalid.", null) { }

    /// <inheritdoc cref="IdentifierInvalidException"/>
    public IdentifierInvalidException() : this(default(Identifier)) { }

    [DoesNotReturn]
    internal static void ThrowIdentifierAlreadyInUse(Identifier id) => throw new IdentifierInvalidException(id, $"Id {id} is already in use!");

}
