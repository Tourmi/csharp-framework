namespace Tourmi.Framework.Collections;

[TestFixture(TestOf = typeof(AutoPool<>))]
internal class AutoPoolUnitTests
{
    private int _factoryCallCount;
    private IPool<object>? _pool;

    private IPool<object> Pool => _pool.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _factoryCallCount = 0;
        _pool = new AutoPool<object>(() =>
        {
            _factoryCallCount++;
            return new object();
        });
    }

    [Test]
    public void TryPopReturnsFalseWhenEmpty()
    {
        var actual = Pool.TryPop(out _);

        Assert.That(actual, Is.False);
    }

    [Test]
    public void TryPopReturnsTrueWhenAnItemExists()
    {
        Pool.Push(new());

        var actual = Pool.TryPop(out _);

        Assert.That(actual, Is.True);
    }

    [Test]
    public void TryPopReturnsOriginalItem()
    {
        var item = new object();
        Pool.Push(item);

        _ = Pool.TryPop(out var actual);

        Assert.That(actual, Is.SameAs(item));
    }

    [Test]
    public void PushAddsItemsOnEachCall()
    {
        Pool.Push(new());
        Pool.Push(new());
        Pool.Push(new());

        Assert.That(Pool.Count, Is.EqualTo(3));
        Assert.That(Pool.Size, Is.EqualTo(3));

        var actual = Pool.TryPop(out _);
        Assert.That(actual, Is.True);
        actual = Pool.TryPop(out _);
        Assert.That(actual, Is.True);
        actual = Pool.TryPop(out _);
        Assert.That(actual, Is.True);

        actual = Pool.TryPop(out _);
        Assert.That(actual, Is.False);
    }

    [Test]
    public void PopReturnsNewItemViaFactory()
    {
        var item1 = Pool.Pop();
        var item2 = Pool.Pop();
        var item3 = Pool.Pop();

        Assert.Multiple(() =>
        {
            Assert.That(item1, Is.Not.Null);
            Assert.That(item2, Is.Not.Null);
            Assert.That(item3, Is.Not.Null);
            Assert.That(_factoryCallCount, Is.EqualTo(3));
            Assert.That(Pool.Size, Is.EqualTo(3));
        });
    }
}
