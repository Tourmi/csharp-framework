namespace Tourmi.Coroutines;

/// <summary>
/// Extension methods for <see cref="CoroutineStatus"/>.
/// </summary>
public static class CoroutineStatusExtensions
{
    /// <summary>
    /// Determines whether or not the <paramref name="status"/> is <see cref="CoroutineStatus.Running"/>.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="status"/> is <see cref="CoroutineStatus.Running"/>, 
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsRunning(this CoroutineStatus status) => status is CoroutineStatus.Running;

    /// <summary>
    /// Determines whether or not the <paramref name="status"/> is considered completed.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> if the <paramref name="status"/> is <see cref="CoroutineStatus.Running"/>, 
    ///     <see langword="true"/> otherwise.
    /// </returns>
    public static bool IsCompleted(this CoroutineStatus status) => status is not CoroutineStatus.Running;

    /// <summary>
    /// Determines whether or not the <paramref name="status"/> is <see cref="CoroutineStatus.Succeeded"/>.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="status"/> is <see cref="CoroutineStatus.Succeeded"/>, 
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsCompletedSuccessfully(this CoroutineStatus status) => status is CoroutineStatus.Succeeded;

    /// <summary>
    /// Determines whether or not the <paramref name="status"/> is <see cref="CoroutineStatus.Failed"/>.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="status"/> is <see cref="CoroutineStatus.Failed"/>, 
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsFailed(this CoroutineStatus status) => status is CoroutineStatus.Failed;

    /// <summary>
    /// Determines whether or not the <paramref name="status"/> is <see cref="CoroutineStatus.Canceled"/>.
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="status"/> is <see cref="CoroutineStatus.Canceled"/>, 
    ///     <see langword="false"/> otherwise.
    /// </returns>
    public static bool IsCanceled(this CoroutineStatus status) => status is CoroutineStatus.Canceled;
}
