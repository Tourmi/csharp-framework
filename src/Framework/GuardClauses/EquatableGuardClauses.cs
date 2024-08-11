using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="IEquatable{T}"/>
/// </summary>
public static class EquatableGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="argument"/> equals <paramref name="other"/>
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [return: NotNullIfNotNull(nameof(argument))]
    public static T ThrowIfEqual<T>(
        this T argument,
        T other,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IEquatable<T>?
    {
        ArgumentOutOfRangeException.ThrowIfEqual(argument, other, paramName);

        return argument;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="argument"/> does not equal <paramref name="other"/>
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [return: NotNullIfNotNull(nameof(argument))]
    public static T ThrowIfNotEqual<T>(
        this T argument,
        T other,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : IEquatable<T>?
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(argument, other, paramName);

        return argument;
    }
}
