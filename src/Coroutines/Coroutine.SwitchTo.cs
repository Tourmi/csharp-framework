using Tourmi.Coroutines.Contexts;
using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

public readonly partial struct Coroutine
{
    /// <summary>
    /// Resumes execution on the given <see cref="CoroutineContext"/> <paramref name="targetContext"/>.
    /// Will immediately resume execution if the <see cref="CoroutineContext.Current"/> context is already the <paramref name="targetContext"/>.
    /// </summary>
    public static Coroutine SwitchTo(CoroutineContext targetContext)
        => SwitchContextCoroutineSource.Create(targetContext.ThrowIfNull()).Coroutine;
}
