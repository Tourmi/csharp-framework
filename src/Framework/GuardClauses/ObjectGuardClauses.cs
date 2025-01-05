using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for any <see cref="object"/>
/// </summary>
public static class ObjectGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is null
    /// </summary>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    public static T ThrowIfNull<T>([NotNull] this T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (value is null)
        {
            throw new ArgumentNullException(valueName);
        }

        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is null
    /// </summary>
    /// <returns>The non-null <paramref name="value"/></returns>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    public static T ThrowIfNull<T>([NotNull] this T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : struct
    {
        if (!value.HasValue)
        {
            throw new ArgumentNullException(valueName);
        }

        return value.Value;
    }
}
