namespace Tourmi.Monogame.Tweens;

[TestFixture(TestOf = typeof(TweenFactory))]
internal class TweenFactoryTests
{
    private TweenFactory? _tweenFactory;
    private TestComponent? _component;

    private TweenFactory TweenFactory => _tweenFactory.ThrowIfNull();
    private TestComponent Component => _component.ThrowIfNull();

    private class TestComponent
    {
        public float SomeProperty { get; set; }
    }

    [SetUp]
    public void SetUp()
    {
        _tweenFactory = new TweenFactory();
        _component = new TestComponent() { SomeProperty = 10f };
    }

    [Test]
    public void WhenTweeningWithLinearCurve_ItShouldHaveExpectedValueAfterUpdates()
    {
        var tweenStatus = TweenFactory.Create<float>(TimeSpan.FromSeconds(10), v => _ = Component.SomeProperty = v, 10f, 20f, CurveFactory.Linear(), looping: false, startPercent: 0);

        TweenFactory.Update(TimeSpan.FromSeconds(2.5));
        Assert.That(Component.SomeProperty, Is.EqualTo(12.5f).Within(0.1f));

        TweenFactory.Update(TimeSpan.FromSeconds(2.5));
        Assert.That(Component.SomeProperty, Is.EqualTo(15f).Within(0.1f));

        TweenFactory.Update(TimeSpan.FromSeconds(100));
        Assert.That(Component.SomeProperty, Is.EqualTo(20f).Within(0.1f));

        Assert.That(tweenStatus.IsCompleted, Is.True);
    }
}
