using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extension methods for <see cref="StrongBox{T}"/>
/// </summary>
public static class StrongBoxExtensions
{
    extension<T>(StrongBox<T>)
    {
        /// <summary>
        /// Returns a boxed version of the default value of <typeparamref name="T"/>.
        /// </summary>
        [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Default member")]
        public static StrongBox<T> Default => new();
    }
}
