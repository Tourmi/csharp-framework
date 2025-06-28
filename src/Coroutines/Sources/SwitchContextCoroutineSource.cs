using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class SwitchContextCoroutineSource
    : CoroutineSource<SwitchContextCoroutineSource, CoroutineUnit>
{
    public static ICoroutineSource<CoroutineUnit> Create(CoroutineContext targetContext)
    {
        if (CoroutineContext.Current == targetContext)
        {
            return CompletedCoroutineSource.Default;
        }

        var source = Get();
        targetContext.Queue(source);

        return source;
    }

    public override bool TryComplete()
    {
        _ = TrySetResult(default);
        return true;
    }
}
