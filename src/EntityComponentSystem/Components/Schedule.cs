namespace Tourmi.EntityComponentSystem.Components;

/// <summary>
/// Schedule that can trigger other schedules or systems (via ticks).
/// </summary>
/// <param name="Period">
/// Time between ticks of the schedule.
/// If zero, the schedule ticks on every incoming tick.
/// </param>
/// <param name="TickRate">
/// Rate at which the schedule outputs ticks.
/// A rate of 1 or less means the schedule outputs ticks on each tick.
/// 2 means it outputs ticks every 2 ticks, etc.
/// </param>
public record struct Schedule(TimeSpan Period, uint TickRate)
{
    /// <summary>
    /// The amount of time accumulated since the last <see cref="Period"/> completion.
    /// Increments <see cref="CurrentTick"/> once it is equal or greater to <see cref="Period"/>.
    /// </summary>
    public TimeSpan TimeSinceLastTick { get; set; }

    /// <summary>
    /// The current tick the period is at. 
    /// Once this value reaches <see cref="TickRate"/>, it gets reset to 0.
    /// </summary>
    public int CurrentTick { get; set; }
}
