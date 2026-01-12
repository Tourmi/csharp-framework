using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides an instance of a component that is promised to be thread safe.
/// </summary>
/// <remarks>
/// <see cref="ThreadSafe{T}"/> does not mean that the type <typeparamref name="T"/> is thread safe,
/// but that access to the <see cref="Instance"/> will always be made in a threadsafe manner,
/// either because the type <typeparamref name="T"/> has built-in thread safety, or because only read access
/// to the instance is needed.
/// </remarks>
public readonly ref struct ThreadSafe<T>(T instance) : IQueryParam<ThreadSafe<T>>
    where T : class
{
    /// <summary>
    /// Instance that is either thread safe, or only read from.
    /// </summary>
    public readonly T Instance { get; } = instance;

    static QueryParamGlobalCache IQueryParam<ThreadSafe<T>>.GetGlobalCache(World world)
    {
        var idCache = QueryParam.GetCache<StrongBox<Identifier>>();

        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<ThreadSafe<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

    static QueryParamArchetypeCache IQueryParam<ThreadSafe<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static ThreadSafe<T> IQueryParam<ThreadSafe<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        var collection = QueryParam.UnsafeCastCache<IComponentCollection<T>>(entry.ArchetypeCache);
        return new(collection[entry.EntityIndex]!);
    }

    static void IQueryParam<ThreadSafe<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
