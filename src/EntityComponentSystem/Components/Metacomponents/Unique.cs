namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Marks a relation type as unique, meaning that an entity may only have a single relation of that type.
/// ie: ChildOf is unique, meaning an entity may only have a single parent.
/// </summary>
[TagComponent]
public readonly record struct Unique;
