using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides a readonly reference for a value of type <typeparamref name="T"/>. 
/// Equivalent to the <see langword="in"/> or <see langword="ref"/> <see langword="readonly"/> keywords.
/// </summary>
public readonly ref struct RefReadonly<T>(ref readonly T reference) : IQueryParam<RefReadonly<T>>
{
    private readonly ref readonly T _reference = ref reference;

    /// <summary>
    /// Target of the reference
    /// </summary>
    public readonly ref readonly T Reference => ref _reference;

    static QueryParamGlobalCache IQueryParam<RefReadonly<T>>.GetGlobalCache(World world)
    {
        var idCache = QueryParam.GetCache<StrongBox<Identifier>>();

        idCache.Value = world.GetEntityForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<RefReadonly<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

    static QueryParamArchetypeCache IQueryParam<RefReadonly<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static RefReadonly<T> IQueryParam<RefReadonly<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        var collection = QueryParam.UnsafeCastCache<IComponentCollection<T>>(entry.ArchetypeCache);
        return new(ref collection[entry.EntityIndex]!);
    }

    static void IQueryParam<RefReadonly<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
