namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.ChildOf"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.ChildOf, IsUnique = true)]
public readonly struct ChildOf;
