namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Built-in event types, for use as generic constraints.
/// </summary>
public static class Events
{
    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.PreTick"/> event.
    /// </summary>
    public readonly record struct PreTick;

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.Tick"/> event.
    /// </summary>
    public readonly record struct Tick;

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.PostTick"/> event.
    /// </summary>
    public readonly record struct PostTick;
}
