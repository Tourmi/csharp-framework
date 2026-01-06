using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Interface for types that have a special handling in queries.
/// </summary>
public interface IQueryParam<T>
    where T : IQueryParam<T>, allows ref struct
{
    /// <summary>
    /// Should return a cache object that will be given to following calls during a query iteration
    /// </summary>
    internal static virtual QueryParamGlobalCache GetGlobalCache(World world) => default;

    /// <summary>
    /// Should free the cache object that was generated with <see cref="GetGlobalCache"/>, 
    /// if needed (such as in the case of object pooling)
    /// </summary>
    internal static virtual void FreeGlobalCache(QueryParamGlobalCache existingCache, World world) { }

    /// <summary>
    /// Should return a cache object that will be passed to the following calls during a query iteration
    /// </summary>
    internal static virtual QueryParamArchetypeCache GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache) => default;

    /// <summary>
    /// Should free the cache object that was generated with <see cref="GetArchetypeCache"/>, 
    /// if needed (such as in the case of object pooling)
    /// </summary>
    internal static virtual void FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype) { }

    /// <summary>
    /// Populates the query parameter with the given information.
    /// </summary>
    internal static abstract T CreateFrom(QueryParamEntityInfo entry);
}
