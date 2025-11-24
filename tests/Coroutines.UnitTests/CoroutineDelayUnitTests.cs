using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines;

[TestFixture(TestOf = typeof(Coroutine))]
internal class CoroutineDelayUnitTests
    : CoroutineBaseTest
{
    [Test]
    public void DelayShouldCompleteImmediatelyIfTimeIsZero()
    {
        var coroutine = Coroutine.Delay(TimeSpan.Zero);

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelayShouldCompleteOnNextFrameIfTimeIsOneSecond()
    {
        var coroutine = Coroutine.Delay(TimeSpan.FromSeconds(1));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelayShouldCompleteIn2FramesIfTimeIsJustOverOneSecond()
    {
        var coroutine = Coroutine.Delay(TimeSpan.FromSeconds(1.5));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelayShouldCompleteProperlyWithVariableDeltaTime()
    {
        var coroutine = Coroutine.Delay(TimeSpan.FromSeconds(2));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        Context.ProvideDeltaTime(TimeSpan.FromSeconds(0.5));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        Context.ProvideDeltaTime(TimeSpan.FromSeconds(1));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        Context.ProvideDeltaTime(TimeSpan.FromSeconds(0.5));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelayShouldBeCancellable()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var coroutine = Coroutine.Delay(TimeSpan.FromSeconds(2), cancellationTokenSource.Token);
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        cancellationTokenSource.Cancel();

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Canceled));
    }

    [Test]
    public void DelaySecondsShouldCompleteImmediatelyIfTimeIsZero()
    {
        var coroutine = Coroutine.DelaySeconds(0);

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelaySecondsShouldCompleteOnNextFrameIfTimeIsOneSecond()
    {
        var coroutine = Coroutine.DelaySeconds(1);
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelaySecondsShouldCompleteIn2FramesIfTimeIsJustOverOneSecond()
    {
        var coroutine = Coroutine.DelaySeconds(1.5);
        NextFrame();

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelaySecondsShouldCompleteProperlyWithVariableDeltaTime()
    {
        var coroutine = Coroutine.DelaySeconds(2);
        NextFrame();

        Context.ProvideDeltaTime(TimeSpan.FromSeconds(0.5));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        Context.ProvideDeltaTime(TimeSpan.FromSeconds(1));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        Context.ProvideDeltaTime(TimeSpan.FromSeconds(0.5));
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
    }

    [Test]
    public void DelaySecondsShouldBeCancellable()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var coroutine = Coroutine.DelaySeconds(2, cancellationTokenSource.Token);
        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        cancellationTokenSource.Cancel();

        NextFrame();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Canceled));
    }
}
