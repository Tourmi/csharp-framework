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
    /// Actions that can be done on the entity.
    /// </summary>
    internal IEntityActions? Actions { get; }
}
