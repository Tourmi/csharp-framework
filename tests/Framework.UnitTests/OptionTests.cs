namespace Tourmi.Framework;

[TestFixture(TestOf = typeof(Option<>))]
internal class OptionTests
{
    [Test]
    public void DefaultOptionHasNoValue()
    {
        Assert.That(default(Option<string>).HasValue, Is.False);
    }

    [Test]
    public void EnumeratorHasValue()
    {
        Option<string> actual = "asdf";
        var hasValue = false;
        foreach (var value in actual)
        {
            hasValue = true;
        }

        Assert.That(hasValue, Is.True);
    }
}
