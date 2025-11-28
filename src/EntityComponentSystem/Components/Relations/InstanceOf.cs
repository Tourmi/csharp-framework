using Tourmi.EntityComponentSystem.Attributes;

namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.InstanceOf"/>
/// </summary>
[ComponentFlag]
[ComponentRelationType(Unique = true)]
public readonly struct InstanceOf;
