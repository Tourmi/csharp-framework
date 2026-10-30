namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// Interface used by <see cref="Relation{TRelation, TTarget}"/>.
/// </summary>
public interface IRelation
{
    /// <summary>
    /// The type of relation (ie: ChildOf, IsA, etc)
    /// </summary>
    internal static abstract Type RelationType { get; }

    /// <summary>
    /// The target of the relation.
    /// </summary>
    internal static abstract Type TargetType { get; }
}
