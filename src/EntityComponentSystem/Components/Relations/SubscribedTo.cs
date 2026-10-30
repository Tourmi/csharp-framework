namespace Tourmi.EntityComponentSystem.Components.Relations;

/// <summary>
/// <inheritdoc cref="BuiltInRelationType.SubscribedTo"/>
/// </summary>
[TagComponent]
[ComponentRelationType((byte)BuiltInRelationType.SubscribedTo)]
public readonly record struct SubscribedTo;
