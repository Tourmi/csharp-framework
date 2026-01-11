using Tourmi.EntityComponentSystem.Attributes;

namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.Requires"/>
/// </summary>
/// <param name="MissingBehavior">Behavior to use when the dependency is missing from the entity</param>
/// <param name="RemovedBehavior">Behavior to use when the dependency is removed from the entity</param>
[ComponentRelationType]
public readonly record struct Requires(
    Requires.DependencyMissingBehavior MissingBehavior = default,
    Requires.DependencyRemovedBehavior RemovedBehavior = default)
{
    /// <summary>
    /// Enum that describes what should be done if the entity does not contain the dependency
    /// </summary>
    public enum DependencyMissingBehavior
    {
        /// <summary>
        /// Add the missing component. Default behavior.
        /// </summary>
        Add = 0,

        /// <summary>
        /// Cancel adding the component.
        /// </summary>
        Cancel = 1,

        /// <summary>
        /// Throw an exception if the dependency is missing.
        /// </summary>
        Panic = 2,
    }

    /// <summary>
    /// Enum that describes what should be done if the dependency is removed from the entity.
    /// </summary>
    public enum DependencyRemovedBehavior
    {
        /// <summary>
        /// Removes this component as well. Default behavior.
        /// </summary>
        RemoveThis = 0,

        /// <summary>
        /// Cancel removing the dependency.
        /// </summary>
        Cancel = 1,

        /// <summary>
        /// Throw an exception if the dependency is removed.
        /// </summary>
        Panic = 2,
    }
}
