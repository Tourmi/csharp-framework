namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{Event, this} metacomponent, defines that the entity will simply remove the component.
/// </summary>
[TagComponent]
[Singleton]
public readonly struct Remove;
