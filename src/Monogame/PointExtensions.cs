using Tourmi.Monogame;

namespace Tourmi.Monogame;

/// <summary>
/// Extension methods for <see cref="Point"/>
/// </summary>
public static class PointExtensions
{
    extension(Point)
    {
        /// <summary>
        /// Negates
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point operator -(Point p) => new(-p.X, -p.Y);
    }

    extension(Point p)
    {
        /// <summary>
        /// Returns a new point with both axes as positive values
        /// </summary>
        public Point Abs() => new(Math.Abs(p.X), Math.Abs(p.Y));

        /// <summary>
        /// Adds the given number to the point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public Point Add(int nb) => new(p.X + nb, p.Y + nb);

        /// <summary>
        /// Adds the x and y values given to the Point's X and Y values.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The new Point</returns>
        public Point Add(int x, int y) => new(p.X + x, p.Y + y);

        /// <summary>
        /// Subtracts the given value from the Point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public Point Sub(int nb) => p - new Point(nb);

        /// <summary>
        /// Multiplies the Point by the given value.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public Point Multiply(int nb) => new(p.X * nb, p.Y * nb);

        /// <summary>
        /// Multiplies each of the Point's components by the given point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="other"></param>
        /// <returns>The new Point</returns>
        public Point Multiply(Point other) => new(p.X * other.X, p.Y * other.Y);

        /// <summary>
        /// Divides the Point by the given number.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public Point Divide(int nb) => p.Divide(nb, nb);

        /// <summary>
        /// Divides the Point's X and Y values by the other point's X and Y values
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns></returns>
        public Point Divide(Point nb) => p.Divide(nb.X, nb.Y);

        /// <summary>
        /// Divides the Point's X and Y values by the given x and y values
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point Divide(int x, int y) => new(p.X / x, p.Y / y);

        /// <summary>
        /// Calls the Modulo function on both of the Point's attributes.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="nb"></param>
        /// <returns>The new Point</returns>
        public Point Modulo(int nb) => new(p.X.Modulo(nb), p.Y.Modulo(nb));

        /// <summary>
        /// Calls the Module function on both of the Point's attributes, using the other point's axises
        /// </summary>
        /// <returns></returns>
        public Point Modulo(Point p2) => new(p.X.Modulo(p2.X), p.Y.Modulo(p2.Y));

        /// <summary>
        /// Returns whether the two points are neighbors (if their distance is exactly equal to 1)
        /// </summary>
        /// <returns></returns>
        public bool IsNeighbor(Point p2) => p.X == p2.X && Math.Abs(p.Y - p2.Y) == 1 || p.Y == p2.Y && Math.Abs(p.X - p2.X) == 1;

        /// <summary>
        /// Returns the cardinal neighbors to this point
        /// </summary>
        public IEnumerable<Point> Neighbors() => [p.Add(1, 0), p.Add(-1, 0), p.Add(0, 1), p.Add(0, -1),];

        /// <summary>
        /// Returns a list of point contained by the perimeter between the two given points.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public IEnumerable<Point> PerimeterPoints(Point p2)
        {
            for (var i = Math.Min(p.X, p2.X); i <= Math.Max(p.X, p2.X); i++)
            {
                for (var j = Math.Min(p.Y, p2.Y); j <= Math.Max(p.Y, p2.Y); j++)
                {
                    if (i == p.X || i == p2.X || j == p.Y || j == p2.Y)
                    {
                        yield return new(i, j);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a rectangle represented by both given points
        /// </summary>
        /// <returns>
        /// Returns a new rectangle, where the location is <paramref name="p"/> 
        ///     and the size is the difference between the two points
        /// </returns>
        public Rectangle GetRectangleToPoint(Point p2) => new(p, p2 - p);
    }

    /// <summary>
    /// Returns the taxicab distance between both points.
    /// </summary>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static int CabDistance(this Point p1, Point p2)
    {
        var diff = (p2 - p1).Abs();
        return diff.X + diff.Y;
    }
}
