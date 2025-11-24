namespace Tourmi.Coroutines.Sources;

/// <summary>
/// Source for <see cref="Coroutine{TResult}"/> execution and completions.
/// </summary>
public interface ICoroutineSource<TResult> :
    ICoroutineResultSource<TResult>
{
    /// <summary>
    /// Returns the Coroutine associated with this source.
    /// </summary>
    Coroutine<TResult> Coroutine { get; }

    /// <summary>
    /// Returns the current status of the <see cref="Coroutine{TResult}"/>.
    /// </summary>
    CoroutineStatus GetStatus();

    /// <summary>
    /// Completes the execution of the <see cref="Coroutine{TResult}"/>.
    /// Will throw any exceptions that occur, and will also throw if the coroutine is not completed.
    /// </summary>
    TResult GetResult();

    /// <summary>
    /// Called when the Coroutine completes.
    /// </summary>
    void OnCompleted(Action continuation);
}
