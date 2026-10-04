using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class WhenAllCoroutineSource
    : CoroutineSource<WhenAllCoroutineSource, CoroutineUnit>
{
    private readonly List<Coroutine> _coroutines = [];

    public static ICoroutineSource<CoroutineUnit> Create(params ReadOnlySpan<Coroutine> coroutines)
    {
        if (coroutines.Length == 0)
        {
            return CompletedCoroutineSource.Default;
        }

        var source = Get();

        source._coroutines.Clear();
        source._coroutines.AddRange(coroutines);

        CoroutineContext.Current.Queue(source);
        return source;
    }

    public override bool TryComplete()
    {
        var anyFailed = false;
        var anyCanceled = false;

        foreach (var coroutine in _coroutines)
        {
            if (coroutine.Status == CoroutineStatus.Running)
            {
                return false;
            }

            if (coroutine.Status is CoroutineStatus.Failed)
            {
                anyFailed = true;
            }

            if (coroutine.Status is CoroutineStatus.Canceled)
            {
                anyCanceled = true;
            }
        }

        if (anyFailed)
        {
            // We don't really care about GC performance when a coroutine fails
            var exceptions = _coroutines.Where(c => c.Exception is not null).Select(c => c.Exception);
            _ = TrySetException(new AggregateException("One or multiple coroutines failed.", exceptions!));
        }
        else if (anyCanceled)
        {
            _ = TrySetCanceled();
        }
        else
        {
            _ = TrySetResult(default);
        }

        _coroutines.Clear();
        return true;
    }
}
