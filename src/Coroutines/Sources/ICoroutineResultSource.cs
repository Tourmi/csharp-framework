namespace Tourmi.Coroutines.Sources;

/// <summary>
/// Coroutine source that allows updating the result of a Coroutine.
/// </summary>
public interface ICoroutineResultSource<in TResult>
{
    /// <summary>
    /// Attempts to set the result of the task.
    /// </summary>
    bool TrySetResult(TResult result);

    /// <summary>
    /// Attempts to set the coroutine as failed with the given exception.
    /// </summary>
    bool TrySetException(Exception exception);

    /// <summary>
    /// Attempts to set the coroutine as canceled with the given token.
    /// </summary>
    bool TrySetCanceled(CancellationToken cancellationToken = default);
}
