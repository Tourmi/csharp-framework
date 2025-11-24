using System.Runtime.CompilerServices;

namespace Tourmi.Coroutines.CompilerServices;

/// <summary>
/// Awaiter for <see cref="Coroutine{TResult}"/>.
/// </summary>
public readonly struct CoroutineAwaiter<TResult>(in Coroutine<TResult> coroutine)
    : ICriticalNotifyCompletion, IEquatable<CoroutineAwaiter<TResult>>
{
    private readonly Coroutine<TResult> _coroutine = coroutine;

    /// <summary>
    /// Whether or not the coroutine has completed its execution.
    /// </summary>
    public bool IsCompleted => _coroutine.Status is not CoroutineStatus.Running;

    /// <inheritdoc/>
    public static bool operator ==(CoroutineAwaiter<TResult> left, CoroutineAwaiter<TResult> right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(CoroutineAwaiter<TResult> left, CoroutineAwaiter<TResult> right) => !(left == right);

    /// <summary>
    /// Completes the execution and returns the result.
    /// </summary>
    public TResult GetResult() => _coroutine.GetResult();

    /// <summary>
    /// Schedules the next continuation.
    /// </summary>
    public void OnCompleted(Action continuation) => UnsafeOnCompleted(continuation);

    /// <inheritdoc cref="OnCompleted(Action)"/>
    public void UnsafeOnCompleted(Action continuation) => _coroutine.OnCompleted(continuation.ThrowIfNull());

    /// <inheritdoc/>
    public bool Equals(CoroutineAwaiter<TResult> other) => _coroutine.Equals(other._coroutine);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is CoroutineAwaiter<TResult> awaiter && Equals(awaiter);

    /// <inheritdoc/>
    public override int GetHashCode() => _coroutine.GetHashCode();
}
