namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnRemove"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnRemove, IsUnique = true)]
internal readonly struct OnRemove;
