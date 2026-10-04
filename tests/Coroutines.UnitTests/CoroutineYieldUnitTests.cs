namespace Tourmi.Coroutines;

[TestFixture(TestOf = typeof(Coroutine))]
internal class CoroutineYieldUnitTests
    : CoroutineBaseTest
{
    [Test]
    public void YieldShouldRunWhenFrameEnds()
    {
        var hasYielded = false;

        Coroutine.Start(YieldFunction);

        Assert.That(hasYielded, Is.False);
        NextFrame();

        Assert.That(hasYielded, Is.True);

        async Coroutine YieldFunction()
        {
            await Coroutine.Yield();
            hasYielded = true;
        }
    }

    [Test]
    public void NextFrameShouldRunOnTheNextFrame()
    {
        var callCount = 0;

        Coroutine.Start(NextFrameFunction);
        NextFrame(); // yield frame
        Assert.That(callCount, Is.Zero);

        NextFrame(); // frame 1
        Assert.That(callCount, Is.EqualTo(1));

        NextFrame(); // frame 2
        Assert.That(callCount, Is.EqualTo(2));

        async Coroutine NextFrameFunction()
        {
            await Coroutine.NextFrame();
            callCount++;
            await Coroutine.NextFrame();
            callCount++;
        }
    }

    [Test]
    public void NextFramesShouldRunAfterTheGivenFrameCount([Range(1, 10)] int frameCount)
    {
        var callCount = 0;

        Coroutine.Start(NextFramesFunction);
        NextFrame(); // yield frame
        Assert.That(callCount, Is.Zero);

        for (var i = 0; i < frameCount; i++)
        {
            NextFrame();
        }

        Assert.That(callCount, Is.EqualTo(1));

        for (var i = 0; i < frameCount; i++)
        {
            NextFrame();
        }

        Assert.That(callCount, Is.EqualTo(2));

        async Coroutine NextFramesFunction()
        {
            await Coroutine.NextFrames(frameCount);
            callCount++;

            await Coroutine.NextFrames(frameCount);
            callCount++;
        }
    }

    [Test]
    public void NextFramesWith0FrameCountShouldJustYield()
    {
        var callCount = 0;

        Coroutine.Start(NextFramesFunction);
        Assert.That(callCount, Is.EqualTo(0));
        NextFrame(); // yield frame
        Assert.That(callCount, Is.EqualTo(2));

        async Coroutine NextFramesFunction()
        {
            await Coroutine.NextFrames(0);
            callCount++;

            await Coroutine.NextFrames(0);
            callCount++;
        }
    }
}
