namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.DependsOn"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.DependsOn)]
public readonly record struct DependsOn;
