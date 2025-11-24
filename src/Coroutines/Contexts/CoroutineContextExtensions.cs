namespace Tourmi.Coroutines.Contexts;

/// <summary>
/// Extension methods for <see cref="CoroutineContext"/>
/// </summary>
public static class CoroutineContextExtensions
{
    /// <summary>
    /// Provides the delta time for this frame.
    /// This should be called only once if the context has a fixed framerate.
    /// </summary>
    public static void ProvideDeltaTime(this CoroutineContext context, TimeSpan deltaTime)
        => context.ThrowIfNull().ProvideValue(new DeltaTime() { Time = deltaTime });

    /// <summary>
    /// Fetches the delta time for this frame.
    /// </summary>
    public static TimeSpan GetDeltaTime(this CoroutineContext context)
        => context.ThrowIfNull().GetValue<DeltaTime>().Time;
}
