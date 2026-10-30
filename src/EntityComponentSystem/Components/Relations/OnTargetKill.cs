namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnTargetKill"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnTargetKill, IsUnique = true)]
internal readonly struct OnTargetKill;
