namespace Tourmi.Framework.Extensions;

[TestFixture(TestOf = typeof(NumberExtensions))]
internal class NumberExtensionsUnitTests
{
    [Test]
    [TestCase(-5, 3, 1)]
    [TestCase(-6, 3, 0)]
    [TestCase(-7, 3, 2)]
    public void ModuloReturnsCorrectValueForNegativeValue(int value, int modulus, int expected)
    {
        var actual = value.Modulo(modulus);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(0, 10, 0)]
    [TestCase(1, 10, 1)]
    [TestCase(12, 10, 2)]
    [TestCase(115, 10, 5)]
    public void ModuloReturnsCorrectValue(int value, int modulus, int expected)
    {
        var actual = value.Modulo(modulus);

        Assert.That(actual, Is.EqualTo(expected));
    }
}
