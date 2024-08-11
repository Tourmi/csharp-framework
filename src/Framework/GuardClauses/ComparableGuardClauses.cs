using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="IComparable{T}"/>
/// </summary>
public static class ComparableGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="argument"/> is less than <paramref name="other"/>.
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfLessThan<T>(
        this T argument,
        T other,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        _ = argument.ThrowIfNull(paramName);
        _ = other.ThrowIfNull();

        ArgumentOutOfRangeException.ThrowIfLessThan(argument, other, paramName);

        return argument;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="argument"/> is less than or equal to <paramref name="other"/>.
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfLessThanOrEqual<T>(
        this T argument,
        T other,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        _ = argument.ThrowIfNull(paramName);
        _ = other.ThrowIfNull();

        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(argument, other, paramName);

        return argument;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="argument"/> is greater than <paramref name="other"/>.
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfGreaterThan<T>(
        this T argument,
        T other,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        _ = argument.ThrowIfNull(paramName);
        _ = other.ThrowIfNull();

        ArgumentOutOfRangeException.ThrowIfGreaterThan(argument, other, paramName);

        return argument;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="argument"/> is greater than or equal to <paramref name="other"/>.
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public static T ThrowIfGreaterThanOrEqual<T>(
        this T argument,
        T other,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IComparable<T>
    {
        _ = argument.ThrowIfNull(paramName);
        _ = other.ThrowIfNull();

        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(argument, other, paramName);

        return argument;
    }
}
