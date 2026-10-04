using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Coroutines;

/// <summary>
/// Possible states that a Coroutine can be in.
/// </summary>
[SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "Zero is the Completed state")]
public enum CoroutineStatus
{
    /// <summary>
    /// The coroutine has successfully finished executing.
    /// </summary>
    Succeeded = 0,

    /// <summary>
    /// The coroutine is currently running.
    /// </summary>
    Running = 1,

    /// <summary>
    /// The coroutine failed.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// The coroutine was canceled.
    /// </summary>
    Canceled = 3,
}
