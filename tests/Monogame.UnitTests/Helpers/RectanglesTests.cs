namespace Tourmi.Monogame.Helpers;

[TestFixture(TestOf = typeof(Rectangles))]
internal class RectanglesTests
{
    [Test]
    public void GetBoundingRectangleReturnsWorstCaseForPosition()
    {
        var position = new Vector2(-1.1f, 2.9f);
        var size = new Vector2(2f, 3f);

        var actual = Rectangles.GetBoundingRectangle(position, size);

        Assert.That(actual, Is.EqualTo(new Rectangle(-2, 2, 3, 4)));
    }

    [Test]
    public void GetBoundingRectangleReturnsWorstCaseForSize()
    {
        var position = new Vector2(1, 1);
        var size = new Vector2(1.1f, 2.05f);

        var actual = Rectangles.GetBoundingRectangle(position, size);

        Assert.That(actual, Is.EqualTo(new Rectangle(1, 1, 2, 3)));
    }

    [Test]
    public void GetBoundingRectangleReturnsUnalteredValuesForIntegers()
    {
        var position = new Vector2(-1, 2);
        var size = new Vector2(3, 4);

        var actual = Rectangles.GetBoundingRectangle(position, size);

        Assert.That(actual, Is.EqualTo(new Rectangle(-1, 2, 3, 4)));
    }

    [Test]
    public void GetBoundingRectangleWithOffsetReturnsProperValues()
    {
        var position = new Vector2(-1, 3);
        var offset = new Vector2(0.1f, -0.1f);
        var size = new Vector2(2, 3);

        var actual = Rectangles.GetBoundingRectangle(position, offset, size);

        Assert.That(actual, Is.EqualTo(new Rectangle(-1, 2, 3, 4)));
    }
}
