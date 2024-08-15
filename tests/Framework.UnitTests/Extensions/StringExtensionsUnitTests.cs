namespace Tourmi.Framework.Extensions;

[TestFixture(TestOf = typeof(StringExtensions))]
internal class StringExtensionsUnitTests
{
    [Test]
    [TestCase("value1", "value1")]
    [TestCase("Value2", "value2")]
    [TestCase("VALUE3", "vALUE3")]
    [TestCase("a", "a")]
    [TestCase("A", "a")]
    [TestCase("\t", "\t")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public void FirstCharacterLowercaseReturnsExpectedValues(string? value, string? expected)
    {
        var actual = value.FirstCharacterLowercase();

        Assert.That(actual, Is.EqualTo(expected));
    }
}
