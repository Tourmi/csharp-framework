namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Interface for base operations on an entity.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Identifier of the entity.
    /// </summary>
    Identifier Id { get; }

    /// <summary>
    /// Instance of the <see cref="EntityComponentSystem.World"/> that the entity resides in.
    /// </summary>
    internal World? World { get; }
}

/// <summary>
/// Generic interface for entities, allows for shared static behavior.
/// </summary>
public interface IEntity<T> : IEntity where T : IEntity<T>, allows ref struct
{
    /// <summary>
    /// Implicitely converts the entity to its Id.
    /// </summary>
    public static abstract implicit operator Identifier(T entity);
}
