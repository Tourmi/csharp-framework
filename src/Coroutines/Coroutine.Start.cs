using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

public readonly partial struct Coroutine
{
    /// <summary>
    /// Queues and starts a new coroutine on the current <see cref="Contexts.CoroutineContext"/>
    /// </summary>
    public static void Start(Func<Coroutine> function)
        => AutoCompleteCoroutineSource.Create(function.ThrowIfNull());
}
