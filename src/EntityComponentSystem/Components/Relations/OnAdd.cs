namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnAdd"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnAdd, IsUnique = true)]
public readonly struct OnAdd;
