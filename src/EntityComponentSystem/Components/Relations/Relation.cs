namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// Component that defines a relation of type <typeparamref name="TRelation"/>, with a target of type <typeparamref name="TTarget"/>.
/// </summary>
[TagComponent]
public readonly struct Relation<TRelation, TTarget>;
