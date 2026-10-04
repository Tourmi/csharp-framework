namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Meta-component that marks an entity as a component which stores data.
/// </summary>
/// <param name="DataType">
/// The datatype associated with the component.
/// </param>
public readonly record struct DataComponent(Type DataType);
