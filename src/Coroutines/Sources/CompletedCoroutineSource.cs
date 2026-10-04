namespace Tourmi.Coroutines.Sources;

internal class CompletedCoroutineSource : ICoroutineSource<CoroutineUnit>
{
    private static readonly CompletedCoroutineSource _default = new();

    private CompletedCoroutineSource()
    {
    }

    public static CompletedCoroutineSource Default => _default;

    public Coroutine<CoroutineUnit> Coroutine => default;

    public CoroutineUnit GetResult() => default;

    public CoroutineStatus GetStatus() => default;

    public void OnCompleted(Action continuation) => continuation();

    public bool TrySetCanceled(CancellationToken cancellationToken = default) => false;
    public bool TrySetException(Exception exception) => false;
    public bool TrySetResult(CoroutineUnit result) => false;
}
