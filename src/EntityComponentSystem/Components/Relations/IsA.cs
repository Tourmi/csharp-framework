namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.IsA"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.IsA)]
public readonly struct IsA;
