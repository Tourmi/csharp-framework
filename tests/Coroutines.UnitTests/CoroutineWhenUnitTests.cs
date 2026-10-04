namespace Tourmi.Coroutines;

[TestFixture(TestOf = typeof(Coroutine))]
internal class CoroutineWhenUnitTests
    : CoroutineBaseTest
{
    [Test]
    public void WhenAllShouldCompleteImmediatelyWithEmptyArray()
    {
        var coroutine = Coroutine.WhenAll();

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void WhenAllShouldCompleteWhenAllCoroutinesComplete()
    {
        var coroutine = Coroutine.WhenAll([Coroutine.DelaySeconds(1), Coroutine.DelaySeconds(2), Coroutine.DelaySeconds(3)]);
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void WhenAnyShouldCompleteWhenAnyCoroutineCompletes()
    {
        var coroutine = Coroutine.WhenAny([Coroutine.DelaySeconds(1), Coroutine.DelaySeconds(2), Coroutine.DelaySeconds(3)]);
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void WhenAnyShouldReturnResultOfFirstCoroutineToComplete()
    {
        var coroutine = Coroutine.WhenAny(WaitThenReturnResult(2), WaitThenReturnResult(3));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));

        var result = coroutine.Result.Result;
        Assert.That(result, Is.EqualTo(200));

        static async Coroutine<int> WaitThenReturnResult(int seconds)
        {
            await Coroutine.DelaySeconds(seconds);
            return seconds * 100;
        }
    }
}
