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

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.OnAdd"/> event.
    /// </summary>
    public readonly record struct OnAdd;

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.OnModified"/> event.
    /// </summary>
    public readonly record struct OnModified;

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.OnRemove"/> event.
    /// </summary>
    public readonly record struct OnRemove;

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.OnKill"/> event.
    /// </summary>
    public readonly record struct OnKill;

    /// <summary>
    /// Type representation for the <see cref="FixedIds.Events.OnTargetKill"/> event.
    /// </summary>
    public readonly record struct OnTargetKill;
}
