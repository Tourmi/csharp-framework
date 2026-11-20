namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnModified"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnModified, IsUnique = true)]
public readonly struct OnModified;
