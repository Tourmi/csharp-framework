namespace Tourmi.Monogame.Helpers;

/// <summary>
/// Helper methods for <see cref="Rectangle"/>
/// </summary>
public static class Rectangles
{
    /// <summary>
    /// Returns the bounding <see cref="Rectangle"/> with the given <paramref name="position"/>, <paramref name="offset"/> and <paramref name="size"/>
    /// </summary>
    /// <returns>Returns the bounding <see cref="Rectangle"/> of the <paramref name="position"/> and <paramref name="size"/></returns>
    public static Rectangle GetBoundingRectangle(Vector2 position, Vector2 offset, Vector2 size)
    {
        var realPos = position + offset;
        var aabb = new Rectangle(
            (int)Math.Floor(realPos.X),
            (int)Math.Floor(realPos.Y),
            (int)Math.Ceiling(Math.Ceiling(realPos.X + size.X) - realPos.X),
            (int)Math.Ceiling(Math.Ceiling(realPos.Y + size.Y) - realPos.Y));

        return aabb;
    }
}
