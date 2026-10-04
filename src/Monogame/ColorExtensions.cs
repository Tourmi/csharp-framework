namespace Tourmi.Monogame;

/// <summary>
/// Extension methods for <see cref="Color"/>
/// </summary>
public static class ColorExtensions
{
    extension(Color)
    {
        /// <summary>
        /// Converts an HSV color to a Color
        /// </summary>
        /// <param name="h">The hue, as a value from 0 to 1 that wraps around.</param>
        /// <param name="s">The saturation, as a value from 0 to 1.</param>
        /// <param name="v">The value, as a value from 0 to 1.</param>
        public static Color FromHSV(float h, float s, float v)
        {
            h = h.Modulo(1.0f);
            s = s.Clamp(0, 1);
            v = v.Clamp(0, 1);

            var c = v * s;
            var x = c * (1 - MathF.Abs(((h * 6) % 2) - 1));
            var m = v - c;

            var (rf, gf, bf) = h switch
            {
                < 1.0f / 6 => (c, x, 0),
                < 2.0f / 6 => (x, c, 0),
                < 3.0f / 6 => (0, c, x),
                < 4.0f / 6 => (0, x, c),
                < 5.0f / 6 => (x, 0, c),
                < 6.0f / 6 => (c, 0, x),
                _ => (0f, 0f, 0f),
            };

            var r = (byte)((rf + m) * 255).Clamp(0, 255);
            var g = (byte)((gf + m) * 255).Clamp(0, 255);
            var b = (byte)((bf + m) * 255).Clamp(0, 255);

            return new Color(r, g, b);
        }
    }

    extension(Color c)
    {
        /// <summary>
        /// Returns the original color, with the red component replaced by <paramref name="r"/>
        /// </summary>
        public Color WithR(byte r) => new(r, c.G, c.B, c.A);
        /// <summary>
        /// Returns the original color, with the green component replaced by <paramref name="g"/>
        /// </summary>
        public Color WithG(byte g) => new(c.R, g, c.B, c.A);
        /// <summary>
        /// Returns the original color, with the blue component replaced by <paramref name="b"/>
        /// </summary>
        public Color WithB(byte b) => new(c.R, c.G, b, c.A);
        /// <summary>
        /// Returns the original color, with the alpha component replaced by <paramref name="a"/>
        /// </summary>
        public Color WithA(byte a) => new(c.R, c.G, c.B, a);
    }
}
