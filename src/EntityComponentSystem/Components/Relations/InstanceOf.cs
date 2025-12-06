using Tourmi.EntityComponentSystem.Attributes;

namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.InstanceOf"/>
/// </summary>
[TagComponent]
[ComponentRelationType(Unique = true)]
public readonly struct InstanceOf;
