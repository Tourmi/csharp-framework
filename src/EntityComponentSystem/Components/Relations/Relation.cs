namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// Acts as a component that defines a relation of type <typeparamref name="TRelation"/>, with a target of type <typeparamref name="TTarget"/>.
/// </summary>
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Needed to avoid reflection in certain cases")]
public readonly struct Relation<TRelation, TTarget> : IRelation
{
    /// <inheritdoc/>
    public static Type RelationType => typeof(TRelation);

    /// <inheritdoc/>
    public static Type TargetType => typeof(TTarget);
}
