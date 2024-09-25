namespace Tourmi.Monogame.Helpers;

/// <summary>
/// Helper methods for <see cref="Rectangle"/>
/// </summary>
public static class Rectangles
{
    /// <summary>
    /// Returns the bounding <see cref="Rectangle"/> with the given <paramref name="position"/>, <paramref name="offset"/> and <paramref name="size"/>
    /// </summary>
    public static Rectangle GetBoundingRectangle(Vector2 position, Vector2 offset, Vector2 size)
        => GetBoundingRectangle(position + offset, size);

    /// <summary>
    /// Returns the bounding <see cref="Rectangle"/> with the given <paramref name="position"/> and <paramref name="size"/>.
    /// </summary>
    public static Rectangle GetBoundingRectangle(Vector2 position, Vector2 size) => new(
            (int)Math.Floor(position.X),
            (int)Math.Floor(position.Y),
            (int)Math.Ceiling(Math.Ceiling(position.X + size.X) - position.X),
            (int)Math.Ceiling(Math.Ceiling(position.Y + size.Y) - position.Y));
}
