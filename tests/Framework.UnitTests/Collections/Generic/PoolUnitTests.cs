namespace Tourmi.Framework.Collections.Generic;

[TestFixture(TestOf = typeof(Pool<>))]
internal class PoolUnitTests
{
    private IPool<object>? _pool;

    private IPool<object> Pool => _pool.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _pool = new Pool<object>();
    }

    [Test]
    public void TryTakeReturnsFalseWhenEmpty()
    {
        var actual = Pool.TryTake(out _);

        Assert.That(actual, Is.False);
    }

    [Test]
    public void TryTakeReturnsTrueWhenAnItemExists()
    {
        Pool.Return(new());

        var actual = Pool.TryTake(out _);

        Assert.That(actual, Is.True);
    }

    [Test]
    public void TryTakeReturnsOriginalItem()
    {
        var item = new object();
        Pool.Return(item);

        _ = Pool.TryTake(out var actual);

        Assert.That(actual, Is.SameAs(item));
    }

    [Test]
    public void ReturnAddsItemsOnEachCall()
    {
        Pool.Return(new());
        Pool.Return(new());
        Pool.Return(new());

        var actual = Pool.TryTake(out _);
        Assert.That(actual, Is.True);
        actual = Pool.TryTake(out _);
        Assert.That(actual, Is.True);
        actual = Pool.TryTake(out _);
        Assert.That(actual, Is.True);

        actual = Pool.TryTake(out _);
        Assert.That(actual, Is.False);
    }
}
