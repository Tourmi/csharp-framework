namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnTargetKill"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnTargetKill, IsUnique = true)]
public readonly struct OnTargetKill;
