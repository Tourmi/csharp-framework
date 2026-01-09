using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tourmi.Framework.Collections;
using Tourmi.Framework.Runtime;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Provides utility methods for constructing query parameters.
/// </summary>
internal static class QueryParam
{
    /// <summary>
    /// Returns a cached static instance of <typeparamref name="T"/>
    /// </summary>
    internal static T GetCache<T>()
        where T : class, new()
    {
        var pool = ThreadStaticProvider<Pool<T>>.Value;
        if (!pool.TryTake(out var cache))
        {
            cache = new();
        }

        return cache;
    }

    /// <summary>
    /// Casts the given <paramref name="cache"/> to the given type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// This function does an unsafe cast for better performance.
    /// </remarks>
    internal static T CastCache<T>(QueryParamGlobalCache cache)
        where T : class
    {
        Debug.Assert(cache.Value is T, "Cache didn't have expected type.");
        return Unsafe.As<T>(cache.Value);
    }

    /// <inheritdoc cref="CastCache{T}(QueryParamGlobalCache)"/>
    internal static T CastCache<T>(QueryParamArchetypeCache cache)
        where T : class
    {
        Debug.Assert(cache.Value is T, "Cache didn't have expected type.");
        return Unsafe.As<T>(cache.Value);
    }

    /// <summary>
    /// Frees the cached instance returned by <see cref="GetCache"/>.
    /// Make sure this is always called once the cache is no longer needed,
    /// to avoid garbage collection.
    /// </summary>
    internal static void FreeCache<T>(T cache)
        where T : class, new()
    {
        var pool = ThreadStaticProvider<Pool<T>>.Value;
        pool.Return(cache);
    }
}
