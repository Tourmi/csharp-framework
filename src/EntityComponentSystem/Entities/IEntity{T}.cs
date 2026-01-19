namespace Tourmi.EntityComponentSystem.Entities;

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
