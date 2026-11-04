namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{Event, this} metacomponent, defines that the world should panic and crash.
/// </summary>
[TagComponent]
[Singleton]
public readonly struct Panic;
