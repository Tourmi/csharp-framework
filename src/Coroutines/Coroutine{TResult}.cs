using System.Runtime.CompilerServices;
using Tourmi.Coroutines.CompilerServices;
using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

/// <summary>
/// Represents a task that will continue its execution on the same thread later on.
/// </summary>
[AsyncMethodBuilder(typeof(AsyncCoroutineMethodBuilder<>))]
public readonly struct Coroutine<TResult> : IEquatable<Coroutine<TResult>>
{
    private readonly TResult? _result;
    private readonly Exception? _exception;
    private readonly CancellationToken _cancellationToken;
    private readonly ICoroutineSource<TResult>? _coroutineSource;

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(TResult result)
    {
        _result = result;
    }

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(Exception exception)
    {
        _exception = exception.ThrowIfNull();
    }

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(ICoroutineSource<TResult> completionSource)
    {
        _coroutineSource = completionSource.ThrowIfNull();
    }

    /// <summary>
    /// Result of this coroutine.
    /// </summary>
    public TResult Result => GetResult();

    /// <summary>
    /// Current state of the coroutine
    /// </summary>
    public CoroutineStatus Status
    {
        get
        {
            if (_coroutineSource is not null)
            {
                return _coroutineSource.GetStatus();
            }

            if (_exception is not null)
            {
                return CoroutineStatus.Failed;
            }

            if (_cancellationToken.IsCancellationRequested)
            {
                return CoroutineStatus.Canceled;
            }

            return CoroutineStatus.Succeeded;
        }
    }

    internal ICoroutineSource<TResult>? CoroutineSource => _coroutineSource;

    internal Exception? Exception => _exception;

    internal CancellationToken CancellationToken => _cancellationToken;

    /// <inheritdoc/>
    public static bool operator ==(Coroutine<TResult> left, Coroutine<TResult> right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Coroutine<TResult> left, Coroutine<TResult> right) => !(left == right);

    /// <summary>
    /// Returns the default awaiter for a coroutine
    /// </summary>
    public readonly CoroutineAwaiter<TResult> GetAwaiter() => new(this);

    /// <inheritdoc/>
    public bool Equals(Coroutine<TResult> other) =>
        _coroutineSource == other._coroutineSource &&
        _exception == other._exception &&
        EqualityComparer<TResult>.Default.Equals(_result, other._result);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Coroutine<TResult> coroutine && Equals(coroutine);

    /// <inheritdoc/>
    public override int GetHashCode() => _coroutineSource?.GetHashCode() ?? _exception?.GetHashCode() ?? _result?.GetHashCode() ?? 0;

    internal TResult GetResult()
    {
        if (_coroutineSource is not null)
        {
            return _coroutineSource.GetResult();
        }

        if (_exception is not null)
        {
            throw _exception;
        }

        _cancellationToken.ThrowIfCancellationRequested();

        return _result!;
    }

    internal void OnCompleted(Action continuation)
    {
        if (_coroutineSource is not null)
        {
            _coroutineSource.OnCompleted(continuation);
            return;
        }

        continuation();
    }
}
