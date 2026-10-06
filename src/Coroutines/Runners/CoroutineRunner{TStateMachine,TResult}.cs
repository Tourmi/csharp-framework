using System.Runtime.CompilerServices;
using Tourmi.Coroutines.Contexts;
using Tourmi.Framework.Collections;

namespace Tourmi.Coroutines.Runners;

internal sealed class CoroutineRunner<TStateMachine, TResult> : ICoroutineRunner<TResult>
    where TStateMachine : IAsyncStateMachine
{
    [ThreadStatic]
    private static Pool<CoroutineRunner<TStateMachine, TResult>>? _runnerPool;

    private TResult? _result;
    private Exception? _exception;
    private CancellationToken _cancellationToken;
    private bool _isCompleted;
    private bool _isDisposed;
    private TStateMachine? _stateMachine;
    private Action? _postContinuation;
    private CoroutineContext? _capturedContext;

    public CoroutineRunner()
    {
        Continuation = MoveNext;
        void MoveNext() => _stateMachine?.MoveNext();
    }

    public Coroutine<TResult> Coroutine => new(this);

    public Action Continuation { get; }

    public static void GetCoroutineRunner(in TStateMachine stateMachine, out ICoroutineRunner<TResult> runner)
    {
        _runnerPool ??= new();
        if (!_runnerPool.TryPop(out var result))
        {
            result = new();
        }

        result._result = default;
        result._exception = default;
        result._cancellationToken = default;
        result._isCompleted = false;
        result._isDisposed = false;
        result._postContinuation = null;
        result._capturedContext = CoroutineContext.Current;

        // The runner must be copied before the state machine gets set.
        // No idea why that is.
        runner = result;
        result._stateMachine = stateMachine;
    }

    /// <inheritdoc/>
    public TResult GetResult()
    {
        try
        {
            if (!_isCompleted)
            {
                throw new InvalidOperationException("The Coroutine was not completed yet.");
            }

            if (_exception is not null)
            {
                throw _exception;
            }

            _cancellationToken.ThrowIfCancellationRequested();

            return _result!;
        }
        finally
        {
            ReturnToPool();
        }
    }

    public CoroutineStatus GetStatus()
    {
        if (!_isCompleted)
        {
            return CoroutineStatus.Running;
        }

        if (_exception is OperationCanceledException)
        {
            return CoroutineStatus.Canceled;
        }

        if (_cancellationToken.IsCancellationRequested)
        {
            return CoroutineStatus.Canceled;
        }

        if (_exception != null)
        {
            return CoroutineStatus.Failed;
        }

        return CoroutineStatus.Succeeded;
    }

    public void OnCompleted(Action continuation)
    {
        _postContinuation = continuation.ThrowIfNull();
    }

    public void ReturnToPool()
    {
        if (_isDisposed)
        {
            return;
        }

        _result = default;
        _exception = null;
        _cancellationToken = default;
        _isCompleted = false;
        _isDisposed = true;
        _stateMachine = default;
        _postContinuation = null;
        _capturedContext = null;

        _runnerPool?.Push(this);
    }

    public bool TryComplete()
    {
        _postContinuation?.Invoke();
        return true;
    }

    public bool TrySetCanceled(CancellationToken cancellationToken = default)
    {
        if (_isCompleted)
        {
            return false;
        }

        _cancellationToken = cancellationToken;
        SetCompletion();
        return true;
    }

    public bool TrySetException(Exception exception)
    {
        if (_isCompleted)
        {
            return false;
        }

        _exception = exception;
        SetCompletion();
        return true;
    }

    public bool TrySetResult(TResult result)
    {
        if (_isCompleted)
        {
            return false;
        }

        _result = result;
        SetCompletion();
        return true;
    }

    private void SetCompletion()
    {
        _isCompleted = true;

        if (_capturedContext != null && CoroutineContext.Current != _capturedContext)
        {
            _capturedContext.Queue(this);
        }
        else
        {
            _postContinuation?.Invoke();
        }
    }
}
