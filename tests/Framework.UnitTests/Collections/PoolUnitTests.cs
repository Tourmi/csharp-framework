namespace Tourmi.Framework.Collections;

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
    public void PopReturnsOriginalItem()
    {
        var item = new object();
        Pool.Push(item);

        var actual = Pool.Pop();

        Assert.That(actual, Is.SameAs(item));
    }

    [Test]
    public void PopThrowsIfEmpty()
    {
        Assert.That(() => Pool.Pop(), Throws.InvalidOperationException);
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
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.False);
            Assert.That(Pool.Size, Is.EqualTo(3));
        });
    }
}
