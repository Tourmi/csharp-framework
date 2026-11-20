namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.OnExtend"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.OnExtend, IsUnique = true)]
public readonly struct OnExtend;
