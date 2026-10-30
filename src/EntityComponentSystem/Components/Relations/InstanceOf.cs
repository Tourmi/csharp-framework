namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.InstanceOf"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.InstanceOf, IsUnique = true)]
public readonly struct InstanceOf;
