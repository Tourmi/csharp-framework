namespace Tourmi.Framework.Extensions;

[TestFixture(TestOf = typeof(EnumerableExtensions))]
internal class EnumerableExtensionsUnitTests
{
    private Random? _random;
    private IEnumerable<int>? _collection;

    private Random Random => _random.ThrowIfNull();
    private IEnumerable<int> Collection => _collection.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _random = new Random();
        _collection = Enumerable.Range(0, 1000).ToList();
    }

    [Test]
    public void OrderByRandomReturnsRandomEnumerable()
    {
        var actual = Collection.OrderByRandom(Random).ToList();

        Assert.That(actual, Is.EquivalentTo(Collection));
        Assert.That(actual, Is.Not.EqualTo(Collection));
    }
}
