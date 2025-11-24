using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines;

[TestFixture(TestOf = typeof(Coroutine))]
internal class CoroutineTests
{
    [Test]
    public void DefaultCoroutineShouldNotThrow()
    {
        var coroutine = default(Coroutine);

        Assert.DoesNotThrowAsync(async () => await coroutine);
    }

    [Test]
    public void ExceptionContructorShouldThrow()
    {
        var context = new CoroutineContext();

        var coroutine = new Coroutine(new InvalidOperationException("Test"));

        Assert.ThrowsAsync<InvalidOperationException>(async () => await coroutine);
    }

    [Test]
    public void CanceledTokenConstructorShouldThrow()
    {
        var context = new CoroutineContext();

        using var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        var coroutine = new Coroutine(tokenSource.Token);

        Assert.ThrowsAsync<OperationCanceledException>(async () => await coroutine);
    }
}
