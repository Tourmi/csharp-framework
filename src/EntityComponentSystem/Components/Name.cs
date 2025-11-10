namespace Tourmi.EntityComponentSystem.Components;

/// <summary>
/// Names an entity, allowing for easier debugging.
/// </summary>
public readonly struct Name()
{
    /// <summary>
    /// Constructs a new instance of the component with the given <paramref name="name"/> value.
    /// </summary>
    public Name(string name) : this()
    {
        Value = name;
    }

    /// <summary>
    /// Name of the entity.
    /// </summary>
    public string Value { get; init; } = string.Empty;
}
