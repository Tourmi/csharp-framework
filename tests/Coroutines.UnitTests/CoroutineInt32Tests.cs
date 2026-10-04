using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines;

[TestFixture(TestOf = typeof(Coroutine<int>))]
internal class CoroutineInt32Tests
{
    [Test]
    public void DefaultCoroutineShouldNotThrow()
    {
        var coroutine = default(Coroutine<int>);

        Assert.DoesNotThrowAsync(async () => await coroutine);
    }

    [Test]
    public void ExceptionContructorShouldThrow()
    {
        var context = new CoroutineContext();

        var coroutine = new Coroutine<int>(new InvalidOperationException("Test"));

        Assert.ThrowsAsync<InvalidOperationException>(async () => await coroutine);
    }

    [Test]
    public void CanceledTokenConstructorShouldThrow()
    {
        var context = new CoroutineContext();

        using var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        var coroutine = new Coroutine<int>(tokenSource.Token);

        Assert.ThrowsAsync<OperationCanceledException>(async () => await coroutine);
    }
}
