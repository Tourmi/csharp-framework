namespace Tourmi.Monogame.Extensions;

[TestFixture(TestOf = typeof(DictionaryWithPointKeyExtensions))]
internal class DictionaryWithPointKeyExtensionsTests
{
    [Test]
    public void SubsetTest()
    {
        var baseDict = new Dictionary<Point, int>()
        {
            [new(0, 0)] = 11,
            [new(0, 1)] = 12,
            [new(0, 2)] = 13,
            [new(0, 3)] = 14,
            [new(1, 0)] = 21,
            [new(1, 1)] = 22,
            [new(1, 2)] = 23,
            [new(1, 3)] = 24,
            [new(2, 0)] = 31,
            [new(2, 1)] = 32,
            [new(2, 2)] = 33,
            [new(2, 3)] = 34,
            [new(3, 0)] = 41,
            [new(3, 1)] = 42,
            [new(3, 2)] = 43,
            [new(3, 3)] = 44,
        };
        Assert.Multiple(() =>
        {
            Assert.That(baseDict.Subset(new Rectangle(1, 1, 2, 2), 9999), Is.EquivalentTo(new Dictionary<Point, int>() { [new(1, 1)] = 22, [new(1, 2)] = 23, [new(2, 1)] = 32, [new(2, 2)] = 33 }));
            Assert.That(baseDict.Subset(new Rectangle(-1, -1, 2, 2), 9999), Is.EquivalentTo(new Dictionary<Point, int>() { [new(-1, -1)] = 9999, [new(-1, 0)] = 9999, [new(0, -1)] = 9999, [new(0, 0)] = 11 }));
        });
    }
}
