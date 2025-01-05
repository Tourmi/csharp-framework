using System.Numerics;

namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extensions for <see cref="INumber{TSelf}"/>
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    /// Computes the modulo of the two given numbers
    /// </summary>
    /// <returns>Strictly positive result</returns>
    public static TSelf Modulo<TSelf>(this TSelf value, TSelf modulus) where TSelf : INumber<TSelf>
    {
        var res = value % modulus;
        if (TSelf.IsNegative(res))
        {
            return res + modulus;
        }

        return res;
    }

    /// <summary>
    /// Clamps the given <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>
    /// </summary>
    public static TSelf Clamp<TSelf>(this TSelf value, TSelf min, TSelf max) where TSelf : INumber<TSelf>
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }
}
