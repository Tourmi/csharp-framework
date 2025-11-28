namespace Tourmi.EntityComponentSystem.Components;

/// <summary>
/// Component that marks an entity as a component which stores data.
/// </summary>
/// <param name="DataType">
/// The datatype associated with the component.
/// </param>
public readonly record struct DataComponent(Type DataType);
