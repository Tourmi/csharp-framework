using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Reflection;

/// <summary>
/// Utility functions for Reflection-related operations
/// </summary>
[RequiresUnreferencedCode("Uses reflection")]
public static class ReflectionUtils
{
    /// <summary>
    /// Returns the types implementing <typeparamref name="T"/> that are not abstract.
    /// </summary>
    /// <returns>types implementing <typeparamref name="T"/></returns>
    public static IEnumerable<Type> GetImplementingTypes<T>()
        => AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && typeof(T).IsAssignableFrom(t) && (t.IsNestedPublic || t.IsPublic)));
}
