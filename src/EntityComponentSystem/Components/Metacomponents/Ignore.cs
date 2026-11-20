namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{OnExtend, Ignore} metacomponent, 
/// defines that the component should be ignored by the new entity.
/// ie: HasComponent(Entity, IgnoredComponent) will always return <see langword="false"/> if the Entity
/// does not have the component, even if entities it inherits from do.
/// </summary>
[TagComponent]
public readonly struct Ignore;
