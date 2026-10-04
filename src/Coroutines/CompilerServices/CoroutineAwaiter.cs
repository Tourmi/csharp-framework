using System.Runtime.CompilerServices;

namespace Tourmi.Coroutines.CompilerServices;

/// <summary>
/// Awaiter for a <see cref="Coroutine"/>.
/// </summary>
public readonly struct CoroutineAwaiter(in Coroutine coroutine)
    : ICriticalNotifyCompletion, IEquatable<CoroutineAwaiter>
{
    private readonly Coroutine _coroutine = coroutine;

    /// <summary>
    /// Whether or not the coroutine has finished.
    /// </summary>
    public bool IsCompleted => _coroutine.Status is not CoroutineStatus.Running;

    /// <inheritdoc/>
    public static bool operator ==(CoroutineAwaiter left, CoroutineAwaiter right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(CoroutineAwaiter left, CoroutineAwaiter right) => !(left == right);

    /// <summary>
    /// Completes the execution of the coroutine.
    /// </summary>
    public void GetResult() => _coroutine.GetResult();

    /// <inheritdoc/>
    public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);

    /// <inheritdoc/>
    public void UnsafeOnCompleted(Action continuation)
        => _coroutine.OnCompleted(continuation.ThrowIfNull());

    /// <inheritdoc/>
    public bool Equals(CoroutineAwaiter other) => _coroutine.Equals(other._coroutine);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CoroutineAwaiter awaiter && Equals(awaiter);

    /// <inheritdoc/>
    public override int GetHashCode() => _coroutine.GetHashCode();
}
