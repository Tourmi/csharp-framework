namespace Tourmi.EntityComponentSystem.Components;

/// <summary>
/// Component that marks an entity as a component which stores data.
/// </summary>
public readonly struct DataComponent
{
    /// <summary>
    /// The datatype associated with the component.
    /// Will be null if the component has no associated data.
    /// </summary>
    public Type? DataType { get; init; }
}
