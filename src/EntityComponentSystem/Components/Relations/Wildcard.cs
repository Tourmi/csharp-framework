namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.Wildcard"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.Wildcard)]
public readonly record struct Wildcard;
