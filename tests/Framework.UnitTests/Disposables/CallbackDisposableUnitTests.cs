namespace Tourmi.Framework.Disposables;

[TestFixture(TestOf = typeof(CallbackDisposable))]
internal class CallbackDisposableUnitTests
{
    [Test]
    public void CallbackGetsCalledWhenDisposed()
    {
        var callCount = 0;

        var callbackDisposable = new CallbackDisposable(Callback);

        Assert.That(callCount, Is.Zero);

        callbackDisposable.Dispose();
        Assert.That(callCount, Is.EqualTo(1));

        callbackDisposable.Dispose();
        Assert.That(callCount, Is.EqualTo(2));

        void Callback()
        {
            callCount++;
        }
    }
}
