using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class WhenAnyCoroutineSource
    : CoroutineSource<WhenAnyCoroutineSource, Coroutine>
{
    protected readonly List<Coroutine> _coroutines = [];

    public static ICoroutineSource<Coroutine> Create(params ReadOnlySpan<Coroutine> coroutines)
    {
        if (coroutines.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coroutines), "At least one coroutine is required.");
        }

        var source = Get();

        source._coroutines.Clear();
        source._coroutines.EnsureCapacity(coroutines.Length);
        foreach (var coroutine in coroutines)
        {
            source._coroutines.Add(coroutine);
        }

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
