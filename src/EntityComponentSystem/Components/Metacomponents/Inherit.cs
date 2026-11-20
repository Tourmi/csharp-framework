namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{OnExtend, this} metacomponent, 
/// defines that the component should be inherited by the new entity.
/// </summary>
[TagComponent]
public readonly struct Inherit;
