namespace Tourmi.Monogame.Extensions;

/// <summary>
/// Extension methods for <see cref="Rectangle"/>
/// </summary>
public static class RectangleExtensions
{
    /// <summary>
    /// Multiplies all the values inside <paramref name="rect"/> by the given <paramref name="number"/>.
    /// </summary>
    /// <returns>The rectangle with adjusted location and size</returns>
    public static Rectangle Multiply(this Rectangle rect, int number) => new(rect.X * number, rect.Y * number, rect.Width * number, rect.Height * number);

    /// <summary>
    /// Divides all the values inside <paramref name="rect"/> by the given <paramref name="number"/>.
    /// </summary>
    /// <returns>The rectangle with adjusted location and size</returns>
    public static Rectangle Divide(this Rectangle rect, int number) => new(rect.X / number, rect.Y / number, rect.Width / number, rect.Height / number);

    /// <summary>
    /// Divides the <paramref name="rect"/> by the <paramref name="value"/>, rounding down the location, and rounding up the size.
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="value"></param>
    /// <returns>The rectangle with adjusted location and size</returns>
    public static Rectangle DivideRounding(this Rectangle rect, float value) => rect.DivideRounding(value, value);

    /// <summary>
    /// Divides the <paramref name="rect"/> by the <paramref name="xValue"/> and <paramref name="yValue"/>, rounding down the location, and rounding up the size.
    /// </summary>
    /// <returns>The rectangle with adjusted location and size</returns>
    public static Rectangle DivideRounding(this Rectangle rect, float xValue, float yValue) => new(
        x: (int)Math.Floor(rect.X / xValue),
        y: (int)Math.Floor(rect.Y / yValue),
        width: (int)Math.Ceiling(rect.Width / xValue),
        height: (int)Math.Ceiling(rect.Height / yValue));

    /// <summary>
    /// Adjusts the size of the <paramref name="rect"/> by the given <paramref name="amount"/>.
    /// </summary>
    /// <returns>The new <see cref="Rectangle"/></returns>
    public static Rectangle AddToSize(this Rectangle rect, Point amount) => new(rect.Location, rect.Size + amount);

    /// <summary>
    /// Adjusts the size of the <paramref name="rect"/> by the given <paramref name="amount"/>.
    /// </summary>
    /// <returns>The new <see cref="Rectangle"/></returns>
    public static Rectangle AddToSize(this Rectangle rect, int amount) => rect.AddToSize(new Point(amount));

    /// <summary>
    /// Returns the bounding rectangle of this rectangle, making sure the size is positive
    /// </summary>
    /// <returns>The bounding rectangle</returns>
    public static Rectangle BoundingRectangle(this Rectangle rect)
    {
        var location = new Point(Math.Min(rect.Left, rect.Right), Math.Min(rect.Top, rect.Bottom));
        var oppositeCorner = new Point(Math.Max(rect.Left, rect.Right), Math.Max(rect.Top, rect.Bottom));
        var size = oppositeCorner - location;
        return new Rectangle(location, size);
    }

    /// <summary>
    /// Creates the biggest possible rectangle from the corners of the two rectangles.
    /// </summary>
    /// <returns>The new rectangle</returns>
    public static Rectangle BoundingRectangle(this Rectangle rect, Rectangle other)
    {
        rect = rect.BoundingRectangle();
        other = other.BoundingRectangle();

        Point pos = new(Math.Min(rect.Left, other.Left), Math.Min(rect.Top, other.Top));
        Point otherCorner = new(Math.Max(rect.Right, other.Right), Math.Max(rect.Bottom, other.Bottom));

        var size = otherCorner - pos;
        return new Rectangle(pos, size);
    }

    /// <summary>
    /// Translates the rectangle using the X and Y values of the given Point.
    /// </summary>
    public static Rectangle Translate(this Rectangle rect, Point p) => new(rect.Location + p, rect.Size);

    /// <summary>
    /// Translates the rectangle by <paramref name="nb"/>, on both axes
    /// </summary>
    public static Rectangle Translate(this Rectangle rect, int nb) => new(rect.Location + new Point(nb), rect.Size);

    /// <summary>
    /// Calls the <see cref="PointExtensions.Modulo(Point, int)"/> function on the rectangle's location. The size remains unchanged.
    /// </summary>
    /// <returns>The new rectangle</returns>
    public static Rectangle Modulo(this Rectangle rect, int nb) => new(rect.Location.Modulo(nb), rect.Size);

    /// <summary>
    /// Calls the <see cref="PointExtensions.Modulo(Point, Point)"/> function for each of the rectangle's X and Y positions with the given point. Size remains unchanged. 
    /// </summary>
    /// <returns></returns>
    public static Rectangle Modulo(this Rectangle rect, Point point) => new(rect.Location.Modulo(point), rect.Size);

    /// <summary>
    /// Returns the accurate center point of the rectangle
    /// </summary>
    /// <returns>The middle point of the rectangle</returns>
    public static Vector2 AccurateCenter(this Rectangle rect) => rect.Location.ToVector2() + rect.Size.ToVector2() * 0.5f;

    /// <summary>
    /// Returns a list of points that contains each point on a Rectangle's perimeter.
    /// </summary>
    /// <returns>A list of points</returns>
    public static IEnumerable<Point> PerimeterPoints(this Rectangle rect) => rect.Location.PerimeterPoints(rect.Location + rect.Size.Add(-1));
}
