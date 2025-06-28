using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Tourmi.Coroutines.Runners;

namespace Tourmi.Coroutines.CompilerServices;

/// <summary>
/// AsyncMethodBuilder for a <see cref="Coroutine{TResult}"/>.
/// </summary>
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No need in an AsyncMethodBuilder")]
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Needed by the compiler.")]
public struct AsyncCoroutineMethodBuilder<TResult>
{
    private TResult? _result;
    private ICoroutineRunner<TResult>? _runner;
    private Exception? _exception;

    /// <summary>
    /// Creates a new <see cref="AsyncCoroutineMethodBuilder{TResult}"/>
    /// </summary>
    public static AsyncCoroutineMethodBuilder<TResult> Create() => default;

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.Task"/>
    public readonly Coroutine<TResult> Task
    {
        get
        {
            if (_runner is not null)
            {
                return _runner.Coroutine;
            }

            if (_exception is not null)
            {
                return new Coroutine<TResult>(_exception);
            }

            return new Coroutine<TResult>(_result!);
        }
    }

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.Start{TStateMachine}(ref TStateMachine)"/>
    public readonly void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.SetStateMachine"/>
    public readonly void SetStateMachine(IAsyncStateMachine stateMachine) { }

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.SetException(Exception)"/>
    public void SetException(Exception exception)
    {
        if (_runner is not null)
        {
            _runner.TrySetException(exception);
        }
        else
        {
            _exception = exception;
        }
    }

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.SetResult"/>
    public void SetResult(TResult result)
    {
        if (_runner is not null)
        {
            _runner.TrySetResult(result);
        }
        else
        {
            _result = result;
        }
    }

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.AwaitOnCompleted{TAwaiter, TStateMachine}(ref TAwaiter, ref TStateMachine)"/>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _runner ??= CoroutineRunner<TStateMachine, TResult>.GetCoroutineRunner(in stateMachine);

        awaiter.OnCompleted(_runner.Continuation);
    }

    /// <inheritdoc cref="AsyncCoroutineMethodBuilder.AwaitUnsafeOnCompleted{TAwaiter, TStateMachine}(ref TAwaiter, ref TStateMachine)"/>
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _runner ??= CoroutineRunner<TStateMachine, TResult>.GetCoroutineRunner(in stateMachine);

        awaiter.UnsafeOnCompleted(_runner.Continuation);
    }
}
