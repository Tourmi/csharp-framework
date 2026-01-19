namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Exposes actions that can be accomplished on entities.
/// </summary>
public interface IEntityActions
{
    /// <summary>
    /// The <see cref="EntityComponentSystem.World"/> these actions point to.
    /// </summary>
    internal World World { get; }

    /// <summary>
    /// Creates new entity in the world.
    /// </summary>
    Entity CreateEntity();

    /// <summary>
    /// Returns whether the <paramref name="entity"/> is alive or not.
    /// </summary>
    bool IsAlive(Identifier entity);

    /// <summary>
    /// Returns whether the <paramref name="id"/> is valid or not.
    /// If the <paramref name="id"/> is disabled, returns <see langword="false"/>.
    /// If the <paramref name="id"/> is an entity, 
    ///     returns <see langword="true"/> if it is alive.
    /// If the <paramref name="id"/> is a relation between two entities,
    ///     returns <see langword="true"/> if the target entity is alive, and the relation exists.
    /// </summary>
    bool IsValid(Identifier id);

    /// <summary>
    /// Kills the given entity.
    /// </summary>
    void Kill(Identifier entity);

    /// <summary>
    /// Returns true if the <paramref name="entity"/> has the given <paramref name="componentId"/>.
    /// </summary>
    bool Has(Identifier entity, Identifier componentId);

    /// <summary>
    /// Returns the <paramref name="component"/> value for the given <paramref name="entity"/>.
    /// </summary>
    T? Get<T>(Identifier entity, Identifier component);

    /// <summary>
    /// Returns a mutable reference of the <paramref name="component"/> for the given <paramref name="entity"/>.
    /// </summary>
    /// <remarks>
    /// Will throw if the entity or the component are not valid.
    /// </remarks>
    ref T? GetMutable<T>(Identifier entity, Identifier component);

    /// <summary>
    /// Adds the given <paramref name="component"/> to the <paramref name="entity"/>, without any associated data.
    /// </summary>
    void Add(Identifier entity, Identifier component);

    /// <summary>
    /// Sets the <paramref name="component"/>'s value for the <paramref name="entity"/> to the given <paramref name="value"/>
    /// </summary>
    void Set<T>(Identifier entity, Identifier component, T value);

    /// <summary>
    /// Removes the given <paramref name="component"/> from the <paramref name="entity"/>.
    /// </summary>
    void Remove(Identifier entity, Identifier component);
}
