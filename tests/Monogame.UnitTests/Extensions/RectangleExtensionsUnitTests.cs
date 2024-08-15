namespace Tourmi.Monogame.Extensions;

[TestFixture(TestOf = typeof(RectangleExtensions))]
internal class RectangleExtensionsUnitTests
{
    public static object[][] RectangleMultiplyTestSource =
    [
        [new Rectangle(0, 0, 0, 0), 0, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 1, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 10, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), -10, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(5, 5, 0, 0), -10, new Rectangle(-50, -50, 0, 0)],
        [new Rectangle(1, 2, 3, 4), 5, new Rectangle(5, 10, 15, 20)],
    ];

    [TestCaseSource(nameof(RectangleMultiplyTestSource))]
    public void MultiplyTest(Rectangle rectangle, int number, Rectangle expected)
    {
        Assert.That(rectangle.Multiply(number), Is.EqualTo(expected));
    }

    public static object[][] RectangleDivideTestSource =
    [
        [new Rectangle(0, 0, 0, 0), 1, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 10, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), -10, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(5, 5, 0, 0), -5, new Rectangle(-1, -1, 0, 0)],
        [new Rectangle(1, 2, 3, 4), 2, new Rectangle(0, 1, 1, 2)],
    ];

    [TestCaseSource(nameof(RectangleDivideTestSource))]
    public void DivideTest(Rectangle rectangle, int number, Rectangle expected)
    {
        Assert.That(rectangle.Divide(number), Is.EqualTo(expected));
    }

    public static object[][] RectangleDivideRoundingTestSource =
    [
        [new Rectangle(0, 0, 0, 0), 1, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 10, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), -10, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(5, 5, 0, 0), -5, new Rectangle(-1, -1, 0, 0)],
        [new Rectangle(1, 2, 3, 4), 2, new Rectangle(0, 1, 2, 2)],
        [new Rectangle(5, 5, 5, 5), 4, new Rectangle(1, 1, 2, 2)],
    ];

    [TestCaseSource(nameof(RectangleDivideRoundingTestSource))]
    public void DivideRoundingTest(Rectangle rectangle, int number, Rectangle expected)
    {
        Assert.That(rectangle.DivideRounding(number), Is.EqualTo(expected));
    }

    public static object[][] RectangleDivideRoundingXYTestSource =
    [
        [new Rectangle(0, 0, 0, 0), 1, 2, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 10, 20, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), -10, -20, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(5, 5, 0, 0), -5, 5, new Rectangle(-1, 1, 0, 0)],
        [new Rectangle(1, 2, 3, 4), 2, 3, new Rectangle(0, 0, 2, 2)],
        [new Rectangle(5, 10, 5, 10), 4, 8, new Rectangle(1, 1, 2, 2)],
    ];

    [TestCaseSource(nameof(RectangleDivideRoundingXYTestSource))]
    public void DivideRoundingXYTest(Rectangle rectangle, int x, int y, Rectangle expected)
    {
        Assert.That(rectangle.DivideRounding(x, y), Is.EqualTo(expected));
    }

    public static object[][] RectangleAdjustSizeXYTestSource =
    [
        [new Rectangle(0, 0, 0, 0), 0, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 1, new Rectangle(0, 0, 1, 1)],
        [new Rectangle(0, 0, 0, 0), -1, new Rectangle(0, 0, -1, -1)],
        [new Rectangle(5, -5, 3, 2), 5, new Rectangle(5, -5, 8, 7)],
    ];

    [TestCaseSource(nameof(RectangleAdjustSizeXYTestSource))]
    public void AdjustSizeXYTest(Rectangle rectangle, int x, Rectangle expected)
    {
        Assert.That(rectangle.AddToSize(x), Is.EqualTo(expected));
    }

    public static object[][] RectangleAdjustSizePointTestSource =
    [
        [new Rectangle(0, 0, 0, 0), new Point(0, 0), new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), new Point(1, 2), new Rectangle(0, 0, 1, 2)],
        [new Rectangle(1, 2, 2, 1), new Point(-3, -5), new Rectangle(1, 2, -1, -4)],
        [new Rectangle(4, 5, 3, 2), new Point(7, 8), new Rectangle(4, 5, 10, 10)],
    ];

    [TestCaseSource(nameof(RectangleAdjustSizePointTestSource))]
    public void AdjustSizePointTest(Rectangle rectangle, Point point, Rectangle expected)
    {
        Assert.That(rectangle.AddToSize(point), Is.EqualTo(expected));
    }

    public static object[][] SelfBoundingRectangleTestsSource =
    [
        [new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 1, 1), new Rectangle(0, 0, 1, 1)],
        [new Rectangle(5, 10, 2, 3), new Rectangle(5, 10, 2, 3)],
        [new Rectangle(-10, -5, 2, 3), new Rectangle(-10, -5, 2, 3)],
        [new Rectangle(-10, -5, -2, -3), new Rectangle(-12, -8, 2, 3)],
    ];
    [TestCaseSource(nameof(SelfBoundingRectangleTestsSource))]
    public void SelfBoundingRectangleTests(Rectangle rectangle, Rectangle expected)
    {
        Assert.That(rectangle.BoundingRectangle(), Is.EqualTo(expected));
    }

    public static object[][] BoundingRectangleTestsSource =
    [
        [new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), new Rectangle(1, 1, 0, 0), new Rectangle(0, 0, 1, 1)],
        [new Rectangle(5, 10, 2, 3), new Rectangle(-5, -10, -2, -3), new Rectangle(-7, -13, 14, 26)],
    ];
    [TestCaseSource(nameof(BoundingRectangleTestsSource))]
    public void BoundingRectangleTests(Rectangle rectangle, Rectangle other, Rectangle expected)
    {
        Assert.That(rectangle.BoundingRectangle(other), Is.EqualTo(expected));
    }

    public static object[][] TranslatePointTestsSource =
    [
        [new Rectangle(0, 0, 0, 0), new Point(0, 0), new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), new Point(5, 10), new Rectangle(5, 10, 0, 0)],
        [new Rectangle(5, 5, 0, 0), new Point(-2, -7), new Rectangle(3, -2, 0, 0)],
        [new Rectangle(5, 5, 5, 5), new Point(-2, -7), new Rectangle(3, -2, 5, 5)],
    ];
    [TestCaseSource(nameof(TranslatePointTestsSource))]
    public void TranslatePointTests(Rectangle rectangle, Point amount, Rectangle expected)
    {
        Assert.That(rectangle.Translate(amount), Is.EqualTo(expected));
    }

    public static object[][] TranslateIntTestsSource =
    [
        [new Rectangle(0, 0, 0, 0), 0, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(0, 0, 0, 0), 5, new Rectangle(5, 5, 0, 0)],
        [new Rectangle(5, 10, 0, 0), -7, new Rectangle(-2, 3, 0, 0)],
        [new Rectangle(5, 10, 5, 5), -7, new Rectangle(-2, 3, 5, 5)],
    ];
    [TestCaseSource(nameof(TranslateIntTestsSource))]
    public void TranslateIntTests(Rectangle rectangle, int amount, Rectangle expected)
    {
        Assert.That(rectangle.Translate(amount), Is.EqualTo(expected));
    }

    public static object[][] ModuloIntTestsSource =
    [
        [new Rectangle(0, 0, 0, 0), 5, new Rectangle(0, 0, 0, 0)],
        [new Rectangle(2, 3, 4, 5), 5, new Rectangle(2, 3, 4, 5)],
        [new Rectangle(-1, -2, 4, 5), 5, new Rectangle(4, 3, 4, 5)],
    ];
    [TestCaseSource(nameof(ModuloIntTestsSource))]
    public void ModuloIntTests(Rectangle rectangle, int amount, Rectangle expected)
    {
        Assert.That(rectangle.Modulo(amount), Is.EqualTo(expected));
    }

    public static object[][] ModuloPointTestsSource =
    [
        [new Rectangle(0, 0, 0, 0), new Point(5, 3), new Rectangle(0, 0, 0, 0)],
        [new Rectangle(2, 3, 4, 5), new Point(5, 3), new Rectangle(2, 0, 4, 5)],
        [new Rectangle(-1, -2, 4, 5), new Point(5, 3), new Rectangle(4, 1, 4, 5)],
    ];
    [TestCaseSource(nameof(ModuloPointTestsSource))]
    public void ModuloPointTests(Rectangle rectangle, Point amount, Rectangle expected)
    {
        Assert.That(rectangle.Modulo(amount), Is.EqualTo(expected));
    }

    [Test]
    [TestCase(0, 0, 0, 0, 0f, 0f)]
    [TestCase(0, 0, 3, 3, 1.5f, 1.5f)]
    [TestCase(1, 2, 3, 4, 2.5f, 4f)]
    [TestCase(-2, -3, 4, 6, 0f, 0f)]
    public void AccurateCenterTests(int x, int y, int width, int height, float expectedX, float expectedY)
    {
        var rect = new Rectangle(x, y, width, height);
        var expected = new Vector2(expectedX, expectedY);

        Assert.That(rect.AccurateCenter(), Is.EqualTo(expected));
    }

    public static object[][] PerimeterPointsTestsSource =
    [
        [new Rectangle(0, 0, 1, 1), new Point[] { new(0, 0) }],
        [new Rectangle(1, 1, 2, 2), new Point[] { new(1, 1), new(1, 2), new(2, 1), new(2, 2), }],
        [new Rectangle(-1, -1, 3, 3), new Point[] { new(-1, -1), new(1, 1), new(-1, 1), new(1, -1), new(-1, 0), new(1, 0), new(0, 1), new(0, -1), }],
    ];
    [TestCaseSource(nameof(PerimeterPointsTestsSource))]
    public void PerimeterPointsTests(Rectangle rectangle, IEnumerable<Point> expected)
    {
        Assert.That(expected, Is.EquivalentTo(rectangle.PerimeterPoints()));
    }
}
