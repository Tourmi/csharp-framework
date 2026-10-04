namespace Tourmi.Framework.Collections;

[TestFixture(TestOf = typeof(LazyPagedArray<>))]
internal class LazyPagedArrayTests
{
    private const byte PageSize = 2;
    private LazyPagedArray<int>? _collection;

    private LazyPagedArray<int> Collection => _collection.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _collection = new(PageSize);
    }

    [Test]
    public void PagedArrayInitializedWithDefaultValues()
    {
        var actual = Collection[1000];

        Assert.That(actual, Is.Default);
    }

    [Test]
    public void PagedArrayInsertSetsValue()
    {
        Collection.Insert(1000, 5);

        var actual = Collection[1000];

        Assert.That(actual, Is.EqualTo(5));
    }

    [Test]
    public void IndexerSetsValue()
    {
        Collection[1000] = 5;

        var actual = Collection[1000];

        Assert.That(actual, Is.EqualTo(5));
    }

    [Test]
    public void RefIndexSetsValue()
    {
        ref var index = ref Collection[1000];
        index = 5;

        var actual = Collection[1000];

        Assert.That(actual, Is.EqualTo(5));
    }
}
