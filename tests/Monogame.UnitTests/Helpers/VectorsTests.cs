namespace Tourmi.Monogame.Helpers;

[TestFixture(TestOf = typeof(Vectors))]
internal class VectorsTests
{
    [Test]
    [TestCase(-1f, 2f)]
    [TestCase(4f, -2.54f)]
    public void WhenCreatingVector2ItIsCreatedProperly(float x, float y)
    {
        var expected = new Vector2(x, y);

        Assert.That(Vectors.Create(x, y), Is.EqualTo(expected));
    }

    [Test]
    [TestCase(-1f, 2f, -3f)]
    [TestCase(4f, -2.54f, 5.64f)]
    public void WhenCreatingVector3ItIsCreatedProperly(float x, float y, float z)
    {
        var expected = new Vector3(x, y, z);

        Assert.Multiple(() =>
        {
            Assert.That(Vectors.Create(x, y, z), Is.EqualTo(expected));
            Assert.That(Vectors.Create(new Vector2(x, y), z), Is.EqualTo(expected));
            Assert.That(Vectors.Create(x, new Vector2(y, z)), Is.EqualTo(expected));
        });
    }

    [Test]
    [TestCase(-1f, 2f, -3f, 4f)]
    [TestCase(4f, -2.54f, 5.64f, -7.523f)]
    public void WhenCreatingVector4ItIsCreatedProperly(float x, float y, float z, float w)
    {
        var expected = new Vector4(x, y, z, w);

        Assert.Multiple(() =>
        {
            Assert.That(Vectors.Create(x, y, z, w), Is.EqualTo(expected));

            Assert.That(Vectors.Create(new Vector2(x, y), z, w), Is.EqualTo(expected));
            Assert.That(Vectors.Create(x, new Vector2(y, z), w), Is.EqualTo(expected));
            Assert.That(Vectors.Create(x, y, new Vector2(z, w)), Is.EqualTo(expected));
            Assert.That(Vectors.Create(new Vector2(x, y), new Vector2(z, w)), Is.EqualTo(expected));

            Assert.That(Vectors.Create(new Vector3(x, y, z), w), Is.EqualTo(expected));
            Assert.That(Vectors.Create(x, new Vector3(y, z, w)), Is.EqualTo(expected));
        });
    }
}
