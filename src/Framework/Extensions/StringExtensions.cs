using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extension methods for <see cref="string"/>
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns the same string with the first character lowercase
    /// </summary>
    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Use of ToLowerCase")]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? FirstCharacterLowercase(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        if (value.Length == 1)
        {
            return value.ToLowerInvariant();
        }

        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}
