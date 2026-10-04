using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

public readonly partial struct Coroutine
{
    /// <summary>
    /// Delays execution for the amount of time specified.
    /// Execution resumes immediately if the given <paramref name="delay"/> is zero or less.
    /// </summary>
    /// <remarks>
    /// Only works if the current context's <see cref="Contexts.CoroutineContextExtensions.ProvideDeltaTime(Contexts.CoroutineContext, TimeSpan)"/> 
    /// function was called.
    /// </remarks>
    public static Coroutine Delay(TimeSpan delay, CancellationToken cancellationToken = default)
        => DelayCoroutineSource.Create(delay, cancellationToken).Coroutine;

    /// <inheritdoc cref="Delay(TimeSpan, CancellationToken)"/>
    public static Coroutine DelaySeconds(double seconds, CancellationToken cancellationToken = default)
        => DelayCoroutineSource.Create(TimeSpan.FromSeconds(seconds), cancellationToken).Coroutine;
}
