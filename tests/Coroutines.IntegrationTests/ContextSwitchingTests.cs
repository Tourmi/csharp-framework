using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines;

internal class ContextSwitchingTests
{
    [Test]
    public void SingleThreadContextSwitching()
    {
        var startingContext = new CoroutineContext();
        var continueContext = new CoroutineContext();

        Coroutine coroutine;
        using (startingContext.Enter())
        {
            coroutine = CoroutineTestMethod();
            Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));
        }

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        startingContext.EnterExit();
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        continueContext.ProvideDeltaTime(TimeSpan.FromSeconds(1));
        continueContext.EnterExit(10);
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        continueContext.EnterExit();

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));
        coroutine.GetResult();

        async Coroutine CoroutineTestMethod()
        {
            Assert.That(CoroutineContext.Current, Is.SameAs(startingContext), "should start on the original context");
            await Coroutine.NextFrame();

            Assert.That(CoroutineContext.Current, Is.SameAs(startingContext), "should still be on the original context");
            await Coroutine.SwitchTo(continueContext);

            Assert.That(CoroutineContext.Current, Is.SameAs(continueContext), "should have switched to the new context");
            await Coroutine.Delay(TimeSpan.FromSeconds(0));
            await Coroutine.Yield();
            await Coroutine.DelaySeconds(5);
            Assert.That(CoroutineContext.Current, Is.SameAs(continueContext), "should remain on the new context");
            await Coroutine.Yield();
            await Coroutine.Delay(TimeSpan.FromSeconds(5));
            await Coroutine.Yield();
            Assert.That(CoroutineContext.Current, Is.SameAs(continueContext), "should remain on the new context");
        }
    }

    [Test]
    public void MultiThreadContextSwitching()
    {
        var mainThread = Thread.CurrentThread;
        var originalContext = new CoroutineContext();
        CoroutineContext? otherThreadContext = null;

        using var mainThreadSemaphore = new Semaphore(0, 1);
        using var otherThreadSemaphore = new Semaphore(0, 1);
        var otherThread = new Thread(OtherThreadFunction);
        otherThread.Start();

        _ = mainThreadSemaphore.WaitOne();
        Coroutine coroutine;

        using (originalContext.Enter())
        {
            coroutine = CoroutineTestMethod();
            Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));
        }

        originalContext.EnterExit(10);
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));

        _ = otherThreadSemaphore.Release();
        _ = mainThreadSemaphore.WaitOne();

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));
        _ = otherThreadSemaphore.Release();
        _ = mainThreadSemaphore.WaitOne();

        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Running));
        originalContext.EnterExit(2);
        Assert.That(coroutine.Status, Is.EqualTo(CoroutineStatus.Succeeded));

        coroutine.GetResult();

        async Coroutine CoroutineTestMethod()
        {
            Assert.That(CoroutineContext.Current, Is.SameAs(originalContext), "should start on the original context");

            await Coroutine.NextFrame();
            Assert.That(CoroutineContext.Current, Is.SameAs(originalContext), "should still be on the original context");

            await SwitchContextThreadsTestMethod();
            Assert.That(Thread.CurrentThread, Is.SameAs(mainThread), "should have returned to the main thread.");
            Assert.That(CoroutineContext.Current, Is.SameAs(originalContext), "should have returned to the original context");

            await Coroutine.NextFrame();
            Assert.That(Thread.CurrentThread, Is.SameAs(mainThread), "should finish on the main thread.");
            Assert.That(CoroutineContext.Current, Is.SameAs(originalContext), "should finish on the original context");
        }

        async Coroutine SwitchContextThreadsTestMethod()
        {
            Assert.That(Thread.CurrentThread, Is.SameAs(mainThread), "should start on the main thread.");
            Assert.That(CoroutineContext.Current, Is.SameAs(originalContext), "should start on the original context");

            await Coroutine.SwitchTo(otherThreadContext!);
            Assert.That(CoroutineContext.Current, Is.SameAs(otherThreadContext), "should be on the other context");
            Assert.That(Thread.CurrentThread, Is.SameAs(otherThread), "should be on the other thread");

            await Coroutine.NextFrames(3);
            await Coroutine.NextFrame();
            await Coroutine.NextFrame();
            Assert.That(CoroutineContext.Current, Is.SameAs(otherThreadContext), "should still be on the other context");
            Assert.That(Thread.CurrentThread, Is.SameAs(otherThread), "should still be on the other thread");
        }

        void OtherThreadFunction()
        {
            otherThreadContext = new CoroutineContext();

            _ = mainThreadSemaphore.Release();
            _ = otherThreadSemaphore.WaitOne();

            otherThreadContext.EnterExit();

            _ = mainThreadSemaphore.Release();
            _ = otherThreadSemaphore.WaitOne();

            otherThreadContext.EnterExit(5);

            _ = mainThreadSemaphore.Release();
        }
    }
}
