using Tourmi.EntityComponentSystem.Attributes;

namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <see cref="BuiltInRelationType.ChildOf"/>
/// </summary>
[ComponentFlag]
[ComponentRelationType(Unique = true)]
public readonly struct ChildOf;
