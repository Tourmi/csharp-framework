using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for any <see cref="object"/>
/// </summary>
public static class ObjectGuardClauses
{
    extension<T>([NotNull] T? value)
#if NET10_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        [return: NotNull]
        public T ThrowIfNull([CallerArgumentExpression(nameof(value))] string? valueName = null) => value switch
        {
            null => throw new ArgumentNullException(valueName),
            _ => value
        };
    }
}
