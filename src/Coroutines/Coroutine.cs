using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Coroutines;

[Flags]
internal enum CoroutineStates
{
    None = 0,
    Completed = 1 << 0,
    Running = 1 << 1,
}

/// <summary>
/// 
/// </summary>
[AsyncMethodBuilder(typeof(CoroutineAsyncMethodBuilder))]
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No need for now")]
public struct Coroutine
{
    private CoroutineStates _state;

    /// <inheritdoc cref="Coroutine"/>
    public Coroutine()
    {
        _state = CoroutineStates.Running;
    }

    /// <summary>
    /// Returns the default awaiter for a coroutine
    /// </summary>
    public readonly CoroutineAwaiter GetAwaiter() => new(in this);

    internal CoroutineStates State
    {
        readonly get => _state;
        set => _state = value;
    }
}

/// <summary>
/// Awaiter for <see cref="Coroutine"/>
/// </summary>
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No need for an Awaiter")]
public readonly struct CoroutineAwaiter
    : INotifyCompletion
{
    private readonly Coroutine _coroutine;

    /// <summary>
    /// Constructs a <see cref="CoroutineAwaiter"/>
    /// </summary>
    public CoroutineAwaiter(in Coroutine coroutine)
    {
        _coroutine = coroutine;
    }

    /// <summary>
    /// Whether the coroutine finished or not
    /// </summary>
    public bool IsCompleted => _coroutine.State == CoroutineStates.None || (_coroutine.State & CoroutineStates.Completed) != 0;

    /// <summary>
    /// 
    /// </summary>
    public void GetResult() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="continuation"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnCompleted(Action continuation) => throw new NotImplementedException();
}

/// <summary>
/// AsyncMethodBuilder for Coroutines
/// </summary>
[SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "No need for an AsyncMethodBuilder")]
public struct CoroutineAsyncMethodBuilder
{
    private bool _isSuccess;

    /// <summary>
    /// Creates a new <see cref="CoroutineAsyncMethodBuilder"/>
    /// </summary>
    /// <returns></returns>
    public static CoroutineAsyncMethodBuilder Create() => default;

    /// <summary>
    /// Task linked to this <see cref="CoroutineAsyncMethodBuilder"/>
    /// </summary>
    public Coroutine Task { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        if (!_isSuccess)
        {
            stateMachine.MoveNext();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateMachine"></param>
    public void SetStateMachine(IAsyncStateMachine stateMachine) { }

    /// <summary>
    /// 
    /// </summary>
    public void SetException(Exception exception) { }

    /// <summary>
    /// Sets the result of the Coroutine
    /// </summary>
    public void SetResult() => _isSuccess = true;

    /// <summary>
    /// 
    /// </summary>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAwaiter"></typeparam>
    /// <typeparam name="TStateMachine"></typeparam>
    /// <param name="awaiter"></param>
    /// <param name="stateMachine"></param>
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {

    }
}
