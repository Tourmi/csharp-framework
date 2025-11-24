using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines.Sources;

internal class AutoCompleteCoroutineSource : CoroutineSource<AutoCompleteCoroutineSource, CoroutineUnit>
{
    private Coroutine _coroutine;

    public static AutoCompleteCoroutineSource Create(Func<Coroutine> coroutine)
    {
        var source = Get();
        source._coroutine = coroutine();

        CoroutineContext.Current.Queue(source);
        return source;
    }

    public override bool TryComplete()
    {
        if (_coroutine.Status is CoroutineStatus.Running)
        {
            return false;
        }

        (var completedCoroutine, _coroutine) = (_coroutine, default);
        var completionStatus = completedCoroutine.Status;

        if (completedCoroutine.Status is CoroutineStatus.Succeeded)
        {
            _ = TrySetResult(default);
        }
        else if (completedCoroutine.Status is CoroutineStatus.Canceled)
        {
            _ = TrySetCanceled(completedCoroutine.CancellationToken);
        }
        else
        {
            _ = TrySetException(completedCoroutine.Exception!);
        }

        try
        {
            _coroutine.GetResult();
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
        {
        }

        // Since this coroutine source will never have GetResult called on it, return it to the pool immediately.
        ReturnToPoolInternal();

        return true;
    }
}
