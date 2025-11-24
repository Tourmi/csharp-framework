using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for any <see cref="object"/>
/// </summary>
public static class ObjectGuardClauses
{
    extension<T>([NotNull] T? value)
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="value"/> is null
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        [return: NotNull]
        public T ThrowIfNull([CallerArgumentExpression(nameof(value))] string? valueName = null)
        {
            if (value is null)
            {
                throw new ArgumentNullException(valueName);
            }

            return value;
        }
    }
}
