namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Built-in event types, for use as generic constraints.
/// </summary>
public static class Events
{
    /// <summary>
    /// Type representation for the Tick event.
    /// </summary>
    public readonly record struct Tick;
}
