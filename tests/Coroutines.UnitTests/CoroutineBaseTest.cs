using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines;

internal abstract class CoroutineBaseTest
{
    private CoroutineContext? _context;

    protected CoroutineContext Context => _context.ThrowIfNull();

    [SetUp]
    public virtual void SetUp()
    {
        _context = new CoroutineContext();

        Context.Enter();
        Context.ProvideDeltaTime(TimeSpan.FromSeconds(1));
    }

    [TearDown]
    public virtual void TearDown()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Context.Exit();
#pragma warning restore CS0618 // Type or member is obsolete
    }

    protected void NextFrame()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Context.Exit();
#pragma warning restore CS0618 // Type or member is obsolete

        Context.Enter();
    }
}
