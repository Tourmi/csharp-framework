using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class YieldFramesCoroutineSource
    : CoroutineSource<YieldFramesCoroutineSource, CoroutineUnit>
{
    private int _targetFrameCount;
    private int _currentFrameCount;

    public static ICoroutineSource<CoroutineUnit> Create(int frameCount, CancellationToken cancellationToken = default)
    {
        var source = Get(cancellationToken);
        source._currentFrameCount = 0;
        source._targetFrameCount = frameCount;

        CoroutineContext.Current.Queue(source);

        return source;
    }

    public override bool TryComplete()
    {
        if (CancellationToken.IsCancellationRequested)
        {
            _ = TrySetCanceled(CancellationToken);
            return true;
        }

        if (_currentFrameCount < _targetFrameCount)
        {
            _currentFrameCount++;
            return false;
        }

        _ = TrySetResult(default);
        return true;
    }
}
