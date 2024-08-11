using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="string"/>
/// </summary>
public static class StringGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when the string is empty,
    /// or a <see cref="ArgumentNullException"/> when the string is null
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    public static string ThrowIfNullOrEmpty([NotNull] this string value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(paramName, nameof(paramName));

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when the string is only whitespace,
    /// or a <see cref="ArgumentNullException"/> when the string is null
    /// </summary>
    /// <returns>The original value</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    public static string ThrowIfNullOrWhiteSpace([NotNull] this string value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(paramName, nameof(paramName));

        return value;
    }
}
