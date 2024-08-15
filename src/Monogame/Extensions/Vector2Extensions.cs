namespace Tourmi.Monogame.Extensions;

/// <summary>
/// Extension methods for <see cref="Vector2"/>
/// </summary>
public static class Vector2Extensions
{
    /// <summary>
    /// Returns the normalized vector
    /// </summary>
    /// <returns>The new vector with normalized components</returns>
    public static Vector2 Normalized(this Vector2 v)
    {
        v.Normalize();
        return v;
    }

    /// <summary>
    /// Rounds down the vector's attributes.
    /// </summary>
    /// <returns>The new vector</returns>
    public static Vector2 Floor(this Vector2 v) => new((float)Math.Floor(v.X), (float)Math.Floor(v.Y));

    /// <summary>
    /// Rounds up the vector's attributes
    /// </summary>
    /// <returns>The new vector</returns>
    public static Vector2 Ceiling(this Vector2 v) => new((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y));

    /// <summary>
    /// Rounds the vector's attributes
    /// </summary>
    /// <returns>The new vector</returns>
    public static Vector2 Round(this Vector2 v) => new((float)Math.Round(v.X), (float)Math.Round(v.Y));

    /// <summary>
    /// Rotates the Vector by the specified angle.
    /// </summary>
    /// <param name="v">This vector</param>
    /// <param name="angle">should be between -1 to 1</param>
    public static Vector2 Rotate(this Vector2 v, float angle)
    {
        angle *= MathHelper.Tau;
        return new Vector2(MathF.Cos(angle) * v.X - MathF.Sin(angle) * v.Y, MathF.Sin(angle) * v.X + MathF.Cos(angle) * v.Y);
    }

    /// <summary>
    /// Returns the angle of the vector, as a value between 0 to 1, 0 being an angle pointing right
    /// </summary>
    public static float Angle01(this Vector2 v)
    {
        if (v == Vector2.Zero)
        {
            return 0;
        }

        return MathF.IEEERemainder(MathF.Atan2(v.Y, v.X), MathF.Tau);
    }
}
