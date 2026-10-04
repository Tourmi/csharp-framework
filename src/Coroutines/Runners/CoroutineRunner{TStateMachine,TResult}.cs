using System.Runtime.CompilerServices;
using Tourmi.Coroutines.Contexts;
using Tourmi.Framework.Collections.Generic;

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

    public static ICoroutineRunner<TResult> GetCoroutineRunner(in TStateMachine stateMachine)
    {
        if (_runnerPool is null)
        {
            _runnerPool = new();
        }

        if (!_runnerPool.TryTake(out var runner))
        {
            runner = new();
        }

        runner._result = default;
        runner._exception = default;
        runner._cancellationToken = default;
        runner._isCompleted = false;
        runner._isDisposed = false;
        runner._stateMachine = stateMachine;
        runner._postContinuation = null;
        runner._capturedContext = CoroutineContext.Current;

        return runner;
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

        _runnerPool?.Return(this);
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
