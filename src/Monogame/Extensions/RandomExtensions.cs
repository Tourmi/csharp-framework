namespace Tourmi.Monogame.Extensions;

/// <summary>
/// XNA-specific Extension methods for <see cref="Random"/>
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Returns a random point within the area of the two points, including the bottom and right edges.
    /// </summary>
    public static Point NextPointInclusive(this Random rand, Point upLeft, Point bottomRight) =>
        new(rand.ThrowIfNull().Next(upLeft.X, bottomRight.X + 1), rand.Next(upLeft.Y, bottomRight.Y + 1));

    /// <summary>
    /// Returns a random point within the perimeter of the two given points.
    /// </summary>
    public static Point NextPointBorder(this Random rand, Point upLeft, Point bottomRight)
    {
        var perimeter = upLeft.PerimeterPoints(bottomRight).ToList();

        var node = rand.ThrowIfNull().Next(perimeter.Count);

        return perimeter.Skip(node).First();
    }

    /// <summary>
    /// Returns a new random Vector2, of a maximum <paramref name="magnitude"/>
    /// </summary>
    /// <param name="rand">The random source</param>
    /// <param name="magnitude">The maximum magnitude</param>
    /// <returns>A new <see cref="Vector2"/></returns>
    public static Vector2 NextVector2(this Random rand, float magnitude = 1)
    {
        _ = rand.ThrowIfNull();

        return magnitude * (new Vector2(rand.NextSingle(), rand.NextSingle()) * 2 - Vector2.One);
    }

    /// <summary>
    /// Returns a new random Vector3, of a maximum <paramref name="magnitude"/>
    /// </summary>
    /// <param name="rand">The random source</param>
    /// <param name="magnitude">The maximum magnitude</param>
    /// <returns>A new <see cref="Vector3"/></returns>
    public static Vector3 NextVector3(this Random rand, float magnitude = 1)
    {
        _ = rand.ThrowIfNull();

        return magnitude * (new Vector3(rand.NextSingle(), rand.NextSingle(), rand.NextSingle()) * 2 - Vector3.One);
    }

    /// <summary>
    /// Returns a new random Vector4, of a maximum <paramref name="magnitude"/>
    /// </summary>
    /// <param name="rand">The random source</param>
    /// <param name="magnitude">The maximum magnitude</param>
    /// <returns>A new <see cref="Vector4"/></returns>
    public static Vector4 NextVector4(this Random rand, float magnitude = 1)
    {
        _ = rand.ThrowIfNull();

        return magnitude * (new Vector4(rand.NextSingle(), rand.NextSingle(), rand.NextSingle(), rand.NextSingle()) * 2 - Vector4.One);
    }
}
