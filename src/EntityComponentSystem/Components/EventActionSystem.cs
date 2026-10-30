using static Tourmi.EntityComponentSystem.Components.EventActionSystem;

namespace Tourmi.EntityComponentSystem.Components;

/// <summary>
/// System that is executed upon an event, such as OnKill or OnTargetKill.
/// </summary>
/// <param name="Action">The action to execute.</param>
public readonly record struct EventActionSystem(EventAction Action)
{
    /// <summary>
    /// Delegate for an action executed upon an event.
    /// </summary>
    /// <param name="world">The world of the system</param>
    /// <param name="eventId">The event that was triggered</param>
    /// <param name="entityId">The id of the related entity</param>
    /// <param name="componentId">The id of the related component</param>
    public delegate void EventAction(World world, Identifier eventId, Identifier entityId, Identifier componentId);
}
