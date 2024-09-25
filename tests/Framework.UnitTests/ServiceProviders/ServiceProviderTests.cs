namespace Tourmi.Framework.ServiceProviders;

[TestFixture(TestOf = typeof(ServiceProvider))]
internal class ServiceProviderTests
{
    private ServiceProvider? _serviceProvider;
    private ServiceProvider ServiceProvider => _serviceProvider.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _serviceProvider = new ServiceProvider();
    }

    [Test]
    public void AddingValueProviderShouldReturnValue()
    {
        ServiceProvider.AddProvider(55);
        var actual = ServiceProvider.GetValue<int>();

        Assert.That(actual, Is.EqualTo(55));
    }

    [Test]
    public void AddingValueProviderShouldNotReturnProviderForWrongType()
    {
        ServiceProvider.AddProvider<IComparable<int>>(55);

        Assert.Throws<KeyNotFoundException>(() => ServiceProvider.GetValue<int>());
    }

    [Test]
    public void AddingFuncProviderShouldBeLazilyEvaluated()
    {
        var wasCalled = false;
        ServiceProvider.AddProvider(() =>
        {
            wasCalled = true;
            return 55;
        });

        Assert.That(wasCalled, Is.False);
        var actual = ServiceProvider.GetValue<int>();
        Assert.Multiple(() =>
        {
            Assert.That(wasCalled, Is.True);
            Assert.That(actual, Is.EqualTo(55));
        });
    }

    [Test]
    public void RemovingProviderShouldMakeValueNoLongerAvailable()
    {
        ServiceProvider.AddProvider(55);

        var actual = ServiceProvider.GetValue<int>();
        Assert.That(actual, Is.EqualTo(55));

        ServiceProvider.RemoveProvider<int>();

        Assert.Throws<KeyNotFoundException>(() => ServiceProvider.GetValue<int>());
    }

    [Test]
    public void InstantiatingFailsIfMoreThanOneConstructor()
    {
        Assert.Throws<InvalidOperationException>(() => ServiceProvider.Instantiate<int>());
    }

    [Test]
    public void InstantiatingAutomaticallyFillsInConstructorWithProvidedValue()
    {
        var wasCalled = false;
        ServiceProvider.AddProvider(() =>
        {
            wasCalled = true;
            return 55;
        });

        Assert.That(wasCalled, Is.False);
        var actual = ServiceProvider.Instantiate<ConstructorThatTakesInteger>();
        Assert.Multiple(() =>
        {
            Assert.That(wasCalled, Is.True);
            Assert.That(actual.Value, Is.EqualTo(55));
        });
    }

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    private class ConstructorThatTakesInteger(int value)
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        public readonly int Value = value;
    }
}
