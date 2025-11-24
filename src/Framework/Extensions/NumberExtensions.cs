using System.Numerics;

namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extensions for <see cref="INumber{TSelf}"/>
/// </summary>
public static class NumberExtensions
{
    extension<T>(T value) where T : INumber<T>
    {
        /// <summary>
        /// Computes the modulo of the two given numbers
        /// </summary>
        /// <returns>Strictly positive result</returns>
        public T Modulo(T modulus)
        {
            var res = value % modulus;
            if (T.IsNegative(res))
            {
                return res + modulus;
            }

            return res;
        }

        /// <summary>
        /// Clamps the given value between <paramref name="min"/> and <paramref name="max"/>
        /// </summary>
        public T Clamp(T min, T max)
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
}
