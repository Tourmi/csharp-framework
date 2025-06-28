using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class WhenAnyCoroutineSource<TResult>
    : CoroutineSource<WhenAnyCoroutineSource<TResult>, Coroutine<TResult>>
{
    protected readonly List<Coroutine<TResult>> _coroutines = [];

    public static ICoroutineSource<Coroutine<TResult>> Create(params ReadOnlySpan<Coroutine<TResult>> coroutines)
    {
        if (coroutines.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coroutines), "At least one coroutine is required.");
        }

        var source = Get();

        source._coroutines.Clear();
        source._coroutines.AddRange(coroutines);

        CoroutineContext.Current.Queue(source);
        return source;
    }

    public sealed override bool TryComplete()
    {
        foreach (var coroutine in _coroutines)
        {
            if (coroutine.Status is not CoroutineStatus.Running)
            {
                _ = TrySetResult(coroutine);
                return true;
            }
        }

        return false;
    }
}
