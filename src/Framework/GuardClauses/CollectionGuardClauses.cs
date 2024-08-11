using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="ICollection{T}"/>
/// </summary>
public static class CollectionGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the given collection was empty, 
    /// or an <see cref="ArgumentNullException"/> if it was null
    /// </summary>
    /// <returns>The original collection</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    public static ICollection<T> ThrowIfNullOrEmpty<T>(
        [NotNull] this ICollection<T> argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        _ = argument.ThrowIfNull(paramName);

        if (argument.Count == 0)
        {
            throw new ArgumentException($"Collection '{paramName}' was empty", paramName);
        }

        return argument;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the given collection contains less than <paramref name="count"/> items,
    /// or an <see cref="ArgumentNullException"/> if it was null.
    /// </summary>
    /// <returns>The original collection</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    [return: NotNull]
    public static ICollection<T> ThrowIfContainsLessThan<T>(
        [NotNull] this ICollection<T> argument,
        int count,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        _ = argument.ThrowIfNull(paramName);

        if (argument.Count < count)
        {
            throw new ArgumentException($"Collection '{paramName}' needs at least {count} items", paramName);
        }

        return argument;
    }
}
