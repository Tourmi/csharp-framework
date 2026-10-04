namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Marks a component as a singleton, meaning the component contains its own value.
/// </summary>
[TagComponent]
public readonly record struct Singleton;
