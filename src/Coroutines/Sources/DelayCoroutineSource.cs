using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class DelayCoroutineSource
    : CoroutineSource<DelayCoroutineSource, CoroutineUnit>
{
    private TimeSpan _delay;
    private TimeSpan _time;

    public static ICoroutineSource<CoroutineUnit> Create(TimeSpan delay, CancellationToken cancellationToken = default)
    {
        if (delay <= TimeSpan.Zero)
        {
            return CompletedCoroutineSource.Default;
        }

        var source = Get(cancellationToken);
        source._delay = delay;

        // by setting time to the negative of the current delta time,
        // we avoid the current frame's delta time from affecting completion
        source._time = -CoroutineContext.Current.GetDeltaTime();

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

        if (_delay <= TimeSpan.Zero)
        {
            _ = TrySetResult(default);
            return true;
        }

        _time += CoroutineContext.Current.GetDeltaTime();
        if (_time < _delay)
        {
            return false;
        }

        _ = TrySetResult(default);
        return true;
    }
}
