namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.IsA"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.IsA, IsReflective = true, IsTransitive = true)]
public readonly struct IsA;
