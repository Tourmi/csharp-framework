namespace Tourmi.Monogame;

[TestFixture(TestOf = typeof(ColorExtensions))]
internal class ColorExtensionsTests
{
    [Test]
    [TestCase(0, 1, 2, 3, 10)]
    [TestCase(255, 255, 255, 255, 0)]
    public void WithRTest(byte r, byte g, byte b, byte a, byte newR)
    {
        var actual = new Color(r, g, b, a);
        Assert.That(actual.WithR(newR), Is.EqualTo(new Color(newR, g, b, a)));
    }

    [Test]
    [TestCase(0, 1, 2, 3, 10)]
    [TestCase(255, 255, 255, 255, 0)]
    public void WithGTest(byte r, byte g, byte b, byte a, byte newG)
    {
        var actual = new Color(r, g, b, a);
        Assert.That(actual.WithG(newG), Is.EqualTo(new Color(r, newG, b, a)));
    }

    [Test]
    [TestCase(0, 1, 2, 3, 10)]
    [TestCase(255, 255, 255, 255, 0)]
    public void WithBTest(byte r, byte g, byte b, byte a, byte newB)
    {
        var actual = new Color(r, g, b, a);
        Assert.That(actual.WithB(newB), Is.EqualTo(new Color(r, g, newB, a)));
    }

    [Test]
    [TestCase(0, 1, 2, 3, 10)]
    [TestCase(255, 255, 255, 255, 0)]
    public void WithATest(byte r, byte g, byte b, byte a, byte newA)
    {
        var actual = new Color(r, g, b, a);
        Assert.That(actual.WithA(newA), Is.EqualTo(new Color(r, g, b, newA)));
    }
}
