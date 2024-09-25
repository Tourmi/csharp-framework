namespace Tourmi.Framework.Extensions;

[TestFixture(TestOf = typeof(RandomExtensions))]
internal class RandomExtensionsTests
{
    private Random? _random;
    private Random Random => _random.ThrowIfNull();

    [SetUp]
    public void SetUp() => _random = new Random();

    [Test]
    public void SingleFromReturnsValue()
    {
        List<int> collection = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        var actual = Random.SingleFrom(collection);

        Assert.That(collection.Contains(actual));
    }

    [Test]
    public void SingleFromThrowsWhenCollectionEmpty()
    {
        List<int> collection = [];

        Assert.Throws<ArgumentException>(() => Random.SingleFrom(collection));
    }

    [Test]
    public void MultipleFromReturnsValues()
    {
        List<int> collection = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        var actual = Random.MultipleFrom(collection, 5).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual.All(collection.Contains));
            Assert.That(actual, Has.Count.EqualTo(5));
        });
    }

    [Test]
    public void MultipleFromThrowsWhenNotEnoughElements()
    {
        List<int> collection = [1, 2, 3, 4, 5, 6, 7, 8, 9];

        Assert.Throws<ArgumentException>(() => Random.MultipleFrom(collection, 10).ToList());
    }
}
