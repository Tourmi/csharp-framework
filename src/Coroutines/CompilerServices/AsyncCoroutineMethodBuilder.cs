using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Tourmi.Coroutines.Runners;

namespace Tourmi.Coroutines.CompilerServices;

/// <summary>
/// AsyncMethodBuilder for <see cref="Coroutine"/>
/// </summary>
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No need for an AsyncMethodBuilder")]
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Needed for AsyncMethodBuilder.")]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Cannot be static for AsyncMethodBuilder.")]
public struct AsyncCoroutineMethodBuilder
{
    private ICoroutineRunner<CoroutineUnit>? _runner;
    private Exception? _exception;

    /// <summary>
    /// Creates a new <see cref="AsyncCoroutineMethodBuilder"/>
    /// </summary>
    public static AsyncCoroutineMethodBuilder Create() => default;

    /// <summary>
    /// Task linked to this <see cref="AsyncCoroutineMethodBuilder"/>.
    /// </summary>
    public readonly Coroutine Task
    {
        get
        {
            if (_runner is not null)
            {
                return _runner.Coroutine;
            }

            if (_exception is not null)
            {
                return new Coroutine(_exception);
            }

            return default;
        }
    }

    /// <summary>
    /// Starts the state machine and captures the current context.
    /// </summary>
    public readonly void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

    /// <remarks>
    /// Unused, causes boxing of the async state machine.
    /// </remarks>
    [Obsolete("Not to be used, since it causes boxing of the state machine.")]
    public readonly void SetStateMachine(IAsyncStateMachine stateMachine) { }

    /// <summary>
    /// Sets the exception of the Coroutine.
    /// </summary>
    public void SetException(Exception exception)
    {
        if (_runner is not null)
        {
            _ = _runner.TrySetException(exception);
        }
        else
        {
            _exception = exception;
        }
    }

    /// <summary>
    /// Sets the result of the Coroutine and completes it.
    /// </summary>
    public readonly void SetResult() => _runner?.TrySetResult(default);

    /// <summary>
    /// Initializes the coroutine runner if needed, and assigns the continuation.
    /// </summary>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _runner ??= CoroutineRunner<TStateMachine, CoroutineUnit>.GetCoroutineRunner(in stateMachine);

        awaiter.OnCompleted(_runner.Continuation);
    }

    /// <inheritdoc cref="AwaitOnCompleted{TAwaiter, TStateMachine}(ref TAwaiter, ref TStateMachine)"/>
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter,
        ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _runner ??= CoroutineRunner<TStateMachine, CoroutineUnit>.GetCoroutineRunner(in stateMachine);

        awaiter.UnsafeOnCompleted(_runner.Continuation);
    }
}
