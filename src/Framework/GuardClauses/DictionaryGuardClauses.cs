using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for a <see cref="IDictionary{TKey, TValue}"/>
/// </summary>
public static class DictionaryGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if it already contains the given key
    /// </summary>
    /// <returns>the original dictionary</returns>
    /// <exception cref="ArgumentException"/>
    public static IDictionary<TKey, TValue> ThrowIfContainsKey<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary,
        TKey key,
        [CallerArgumentExpression(nameof(dictionary))] string? paramName = null)
        where TKey : notnull
    {
        _ = dictionary.ThrowIfNull(paramName);

        if (dictionary.ContainsKey(key))
        {
            throw new ArgumentException($"Dictionary '{paramName}' already contains key '{key}'", paramName);
        }

        return dictionary;
    }
}
