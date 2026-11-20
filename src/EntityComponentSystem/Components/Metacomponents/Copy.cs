namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// When part of a Relation{OnExtend, Copy} metacomponent,
/// defines that the component should be copied into the new entity upon inheritance.
/// This is the default behavior.
/// </summary>
[TagComponent]
public readonly struct Copy;
