namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.ChildOf"/>
/// </summary>
[TagComponent]
[ComponentRelationType(Unique = true)]
public readonly struct ChildOf;
