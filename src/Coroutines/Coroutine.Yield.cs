using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

public readonly partial struct Coroutine
{
    /// <summary>
    /// Yields the execution of the current coroutine to later in the current frame.
    /// </summary>
    public static Coroutine Yield(CancellationToken cancellationToken = default)
        => YieldFramesCoroutineSource.Create(0, cancellationToken).Coroutine;

    /// <summary>
    /// Resumes the coroutine on the next frame.
    /// </summary>
    public static Coroutine NextFrame(CancellationToken cancellationToken = default)
        => YieldFramesCoroutineSource.Create(1, cancellationToken).Coroutine;

    /// <summary>
    /// Resumes the coroutine on the next frames.
    /// </summary>
    public static Coroutine NextFrames(int frameCount, CancellationToken cancellationToken = default)
        => YieldFramesCoroutineSource.Create(frameCount, cancellationToken).Coroutine;
}
