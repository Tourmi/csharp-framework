namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnAdd"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnAdd, IsUnique = true)]
internal readonly struct OnAdd;
