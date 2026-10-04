namespace Tourmi.Coroutines;

/// <summary>
/// Extension methods for <see cref="Coroutine"/>
/// </summary>
public static class CoroutineExtensions
{
    /// <summary>
    /// Converts a <see cref="Coroutine{CoroutineUnit}"/> to a <see cref="Coroutine"/>
    /// </summary>
    public static Coroutine ToCoroutine(this Coroutine<CoroutineUnit> coroutine)
        => new(coroutine.CoroutineSource, coroutine.Exception, coroutine.CancellationToken);
}
