using Microsoft.Xna.Framework;

namespace Tourmi.Samples.Particles.Components;

internal readonly record struct Size(float Width, float Height)
{
    public static Size operator *(float scalar, Size size) => new(size.Width * scalar, size.Height * scalar);
    public static Size operator *(Size size, float scalar) => scalar * size;
    public static Size operator /(Size size, float scalar) => new(size.Width / scalar, size.Height / scalar);

    public Point ToPoint() => new((int)Width, (int)Height);
}
