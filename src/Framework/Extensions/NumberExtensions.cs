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
    /// <returns></returns>
    public static TSelf Modulo<TSelf>(this TSelf value, TSelf modulus) where TSelf : INumber<TSelf>
    {
        var res = value % modulus;
        if (TSelf.IsNegative(res))
        {
            return res + modulus;
        }

        return res;
    }
}
