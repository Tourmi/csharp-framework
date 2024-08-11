using System.Numerics;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="INumberBase{T}"/>
/// </summary>
public static class NumberBaseGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfZero<T>(this T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfZero(value, valueName);

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfNegative<T>(this T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value, valueName);
        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative or zero
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfNegativeOrZero<T>(this T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : INumberBase<T>
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, valueName);
        return value;
    }
}
