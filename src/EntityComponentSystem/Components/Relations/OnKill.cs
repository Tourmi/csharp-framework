namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnKill"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnKill, IsUnique = true)]
internal readonly struct OnKill;
