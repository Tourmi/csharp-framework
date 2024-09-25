namespace Tourmi.Framework.ServiceProviders;

[TestFixture(TestOf = typeof(ServiceProviderExtensions))]
internal class ServiceProviderExtensionsTests
{
    private ServiceProvider? _serviceProvider;
    private ServiceProvider ServiceProvider => _serviceProvider.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _serviceProvider = new ServiceProvider();
    }

    [Test]
    public void AddingProviderWithInterfaceShouldRegisterInterface()
    {
        ServiceProvider.AddProvider(55);
        ServiceProvider.AddProvider<ISomeInterface, ConstructorThatTakesInteger>();

        var actual = ServiceProvider.GetValue<ISomeInterface>();

        Assert.That(actual.Value, Is.EqualTo(55));
    }

    [Test]
    public void AddingProviderWithJustTheTypeShouldReturnProperValue()
    {
        ServiceProvider.AddProvider(55);
        ServiceProvider.AddProvider<ConstructorThatTakesInteger>();

        var actual = ServiceProvider.GetValue<ConstructorThatTakesInteger>();

        Assert.That(actual.Value, Is.EqualTo(55));
    }

    [Test]
    public void AddingProviderThatRequiresServiceProviderShouldWork()
    {
        ServiceProvider.AddProvider(55);
        ServiceProvider.AddProvider<ISomeInterface, ConstructorThatTakesInteger>((serviceProvider) => new ConstructorThatTakesInteger(serviceProvider.GetValue<int>()));

        var actual = ServiceProvider.GetValue<ISomeInterface>();

        Assert.That(actual.Value, Is.EqualTo(55));
    }

    [Test]
    public void AddingLazyProviderShouldWorkProperly()
    {
        ServiceProvider.AddProvider(55);
        ServiceProvider.AddLazyProvider<double, LazyProvider>();

        var actual = ServiceProvider.GetValue<double>();

        Assert.That(actual, Is.EqualTo(55 * 2));
    }

    [Test]
    public void ReusingProviderShouldWorkProperly()
    {
        ServiceProvider.AddProvider(55);
        ServiceProvider.ReuseProvider<IComparable<int>, int>();

        var actual = ServiceProvider.GetValue<IComparable<int>>();

        Assert.That((int)actual, Is.EqualTo(55));
    }

    [Test]
    public void SettingValueShouldWorkProperly()
    {
        ServiceProvider.AddProvider(55);
        ServiceProvider.SetValue(66);

        var actual = ServiceProvider.GetValue<int>();

        Assert.That(actual, Is.EqualTo(66));
    }

    [Test]
    public void GettingValueShouldReturnValue()
    {
        ServiceProvider.AddProvider(55);

        var actual = ServiceProvider.GetValue<int>();

        Assert.That(actual, Is.EqualTo(55));
    }

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    private class ConstructorThatTakesInteger(int value) : ISomeInterface
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        public int Value { get; } = value;
    }

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
    private class LazyProvider(int value) : ILazyProvider<double>
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
    {
        public double Value { get; set; } = value * 2;
        public double GetValue() => Value;
        public void SetValue(double value) => Value = value;
    }

    private interface ISomeInterface
    {
        int Value { get; }
    }
}
