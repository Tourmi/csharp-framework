namespace Tourmi.Coroutines.Sources;

/// <summary>
/// Represents a CoroutineSource that should be evaluated within a Game Loop.
/// </summary>
public interface IQueueAble
{
    /// <summary>
    /// Attempts to continue the execution of the CoroutineSource, 
    /// returning <see langword="false"/> if the CoroutineSource hasn't completed yet, and should remain in the execution queue.
    /// </summary>
    bool TryComplete();

    /// <summary>
    /// Called when the Coroutine completes.
    /// </summary>
    void OnCompleted(Action continuation);
}
