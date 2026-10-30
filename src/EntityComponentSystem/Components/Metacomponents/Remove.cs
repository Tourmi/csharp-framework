namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{Event, this} metacomponent, defines that the entity will simply remove the component.
/// </summary>
[TagComponent]
public readonly struct Remove;

/// <summary>
/// When part of a Relation{Event, this} metacomponent, defines that the entity will be killed.
/// </summary>
[TagComponent]
public readonly struct Kill;

/// <summary>
/// When part of a Relation{Event, this} metacomponent, defines that the world should panic and crash.
/// </summary>
[TagComponent]
public readonly struct Panic;
