namespace Tourmi.Monogame;

/// <summary>
/// Extension methods for <see cref="Vector2"/>
/// </summary>
public static class Vector2Extensions
{
    extension(Vector2 v)
    {
        /// <summary>
        /// Returns the normalized vector
        /// </summary>
        /// <returns>The new vector with normalized components</returns>
        public Vector2 Normalized()
        {
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Rounds down the vector's attributes.
        /// </summary>
        /// <returns>The new vector</returns>
        public Vector2 Floored() => new((float)Math.Floor(v.X), (float)Math.Floor(v.Y));

        /// <summary>
        /// Rounds up the vector's attributes
        /// </summary>
        /// <returns>The new vector</returns>
        public Vector2 Ceiling() => new((float)Math.Ceiling(v.X), (float)Math.Ceiling(v.Y));

        /// <summary>
        /// Rounds the vector's attributes
        /// </summary>
        /// <returns>The new vector</returns>
        public Vector2 Round() => new((float)Math.Round(v.X), (float)Math.Round(v.Y));

        /// <summary>
        /// Rotates the Vector by the specified angle.
        /// </summary>
        /// <param name="angle">should be between a number between -1 and 1, where 1 represents a full rotation</param>
        public Vector2 Rotate(float angle)
        {
            angle *= MathHelper.Tau;
            return new Vector2(MathF.Cos(angle) * v.X - MathF.Sin(angle) * v.Y, MathF.Sin(angle) * v.X + MathF.Cos(angle) * v.Y);
        }

        /// <summary>
        /// Returns the angle of the vector, as a value between 0 to 1, 0 being an angle pointing towards positive X
        /// </summary>
        public float Angle01()
        {
            if (v == Vector2.Zero)
            {
                return 0;
            }

            return MathF.IEEERemainder(MathF.Atan2(v.Y, v.X), MathF.Tau) / MathF.Tau;
        }

        /// <summary>
        /// Converts the vector to a point, casting its X and Y components to integers.
        /// </summary>
        public Point ToPoint() => new((int)v.X, (int)v.Y);
    }
}
