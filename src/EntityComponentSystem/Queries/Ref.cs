using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides a reference to a value of type <typeparamref name="T"/>.
/// Equivalent to a <see langword="ref"/> parameter.
/// </summary>
public readonly ref struct Ref<T>(ref T reference) : IQueryParam<Ref<T>>
{
    private readonly ref T _reference = ref reference;

    /// <summary>
    /// Target of the reference.
    /// </summary>
    public ref T Reference => ref _reference;

    static QueryParamGlobalCache IQueryParam<Ref<T>>.GetGlobalCache(World world)
    {
        var idCache = QueryParam.GetCache<StrongBox<Identifier>>();
        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<Ref<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

    static QueryParamArchetypeCache IQueryParam<Ref<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static Ref<T> IQueryParam<Ref<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        var collection = QueryParam.UnsafeCastCache<IComponentCollection<T>>(entry.ArchetypeCache);
        return new(ref collection[entry.EntityIndex]!);
    }

    static void IQueryParam<Ref<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
