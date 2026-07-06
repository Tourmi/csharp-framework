namespace Tourmi.EntityComponentSystem.Components;

/// <summary>
/// Represents a system that can be executed, usually every frame.
/// </summary>
public readonly struct SystemComponent
{
    internal SystemComponent(Action execute)
    {
        Execute = execute;
    }

    /// <summary>
    /// Executes the system.
    /// </summary>
    public Action Execute { get; }

    /// <inheritdoc/>
    public static SystemComponent Create(Action value) => new(value);
}
