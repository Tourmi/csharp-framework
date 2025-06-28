using System.Diagnostics.CodeAnalysis;
using Tourmi.Coroutines.Collections;

namespace Tourmi.Coroutines.Sources;

/// <inheritdoc cref="ICoroutineSource{TResult}"/>
public abstract class CoroutineSource<TSelf, TResult>
    : ICoroutineSource<TResult>, IQueueAble
    where TSelf : CoroutineSource<TSelf, TResult>, new()
{
    private TResult? _result;
    private Exception? _exception;
    private CancellationToken _cancellationToken;
    private CoroutineStatus _status = CoroutineStatus.Running;
    private Action? _continuation;

    /// <summary>
    /// Constructs this coroutine source.
    /// </summary>
    protected CoroutineSource()
    {
    }

    /// <inheritdoc/>
    public Coroutine<TResult> Coroutine => new(this);

    /// <summary>
    /// Cancellation Token for this CoroutineSource
    /// </summary>
    public CancellationToken CancellationToken => _cancellationToken;

    /// <summary>
    /// Returns a pooled instance of this Coroutine Source.
    /// </summary>
    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Needed for implementations")]
    protected static TSelf Get(CancellationToken cancellationToken = default)
    {
        if (!ThreadStaticPools<TSelf>.Get().TryTake(out var source))
        {
            source = new TSelf();
        }

        source._cancellationToken = cancellationToken;
        source._status = CoroutineStatus.Running;
        source._exception = null;
        source._result = default;
        source._continuation = null;

        return source;
    }

    /// <inheritdoc/>
    public abstract bool TryComplete();

    /// <inheritdoc/>
    public TResult GetResult()
    {
        try
        {
            return _status switch
            {
                CoroutineStatus.Succeeded => _result!,
                CoroutineStatus.Failed => throw _exception!,
                CoroutineStatus.Canceled => throw new OperationCanceledException(_cancellationToken),
                CoroutineStatus.Running => throw new InvalidOperationException("Coroutine is still being executed"),
                _ => throw new InvalidOperationException("Reached an impossible Coroutine state"),
            };
        }
        finally
        {
            ReturnToPoolInternal();
        }
    }

    /// <inheritdoc/>
    public CoroutineStatus GetStatus() => _status;

    /// <inheritdoc/>
    public void OnCompleted(Action continuation)
    {
        _continuation = continuation.ThrowIfNull();

        if (_status is CoroutineStatus.Running)
        {
            return;
        }

        continuation.Invoke();
    }

    /// <inheritdoc/>
    public bool TrySetCanceled(CancellationToken cancellationToken = default)
    {
        if (_status is not CoroutineStatus.Running)
        {
            return false;
        }

        _cancellationToken = cancellationToken;

        return TrySetCompletion(CoroutineStatus.Canceled);
    }

    /// <inheritdoc/>
    public bool TrySetException(Exception exception)
    {
        _ = exception.ThrowIfNull();

        if (_status is not CoroutineStatus.Running)
        {
            return false;
        }

        if (exception is OperationCanceledException oce)
        {
            return TrySetCanceled(oce.CancellationToken);
        }

        _exception = exception;
        return TrySetCompletion(CoroutineStatus.Failed);
    }

    /// <inheritdoc/>
    public bool TrySetResult(TResult result)
    {
        if (_status is not CoroutineStatus.Running)
        {
            return false;
        }

        _result = result;
        return TrySetCompletion(CoroutineStatus.Succeeded);
    }

    /// <summary>
    /// Returns this coroutine source to the pool
    /// </summary>
    private protected void ReturnToPoolInternal()
    {
        _result = default;
        _exception = null;
        _cancellationToken = default;
        _status = CoroutineStatus.Running;

        ThreadStaticPools<TSelf>.Get().Return((TSelf)this);
    }

    private bool TrySetCompletion(CoroutineStatus status)
    {
        _status = status;

        _continuation?.Invoke();

        return true;
    }
}
