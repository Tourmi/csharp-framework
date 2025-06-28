namespace Tourmi.Coroutines.Contexts;

/// <summary>
/// Struct to store the current delta time.
/// </summary>
/// <remarks>
/// Used by <see cref="Coroutine.Delay(TimeSpan, CancellationToken)"/>.
/// </remarks>
internal readonly struct DeltaTime
{
    public TimeSpan Time { get; init; }
}
