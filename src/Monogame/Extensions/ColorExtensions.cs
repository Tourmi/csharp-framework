namespace Tourmi.Monogame.Extensions;

/// <summary>
/// Extension methods for <see cref="Color"/>
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Returns the original color, with the red component replaced by <paramref name="r"/>
    /// </summary>
    public static Color WithR(this Color c, byte r) => new(r, c.G, c.B, c.A);
    /// <summary>
    /// Returns the original color, with the green component replaced by <paramref name="g"/>
    /// </summary>
    public static Color WithG(this Color c, byte g) => new(c.R, g, c.B, c.A);
    /// <summary>
    /// Returns the original color, with the blue component replaced by <paramref name="b"/>
    /// </summary>
    public static Color WithB(this Color c, byte b) => new(c.R, c.G, b, c.A);
    /// <summary>
    /// Returns the original color, with the alpha component replaced by <paramref name="a"/>
    /// </summary>
    public static Color WithA(this Color c, byte a) => new(c.R, c.G, c.B, a);
}
