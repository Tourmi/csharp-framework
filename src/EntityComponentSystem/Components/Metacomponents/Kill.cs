namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{Event, this} metacomponent, defines that the entity will be killed.
/// </summary>
[TagComponent]
[Singleton]
public readonly struct Kill;
