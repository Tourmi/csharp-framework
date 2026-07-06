using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides a write-only reference to a value of type <typeparamref name="T"/>.
/// Equivalent to the <see langword="out"/> keyword.
/// </summary>
public readonly ref struct OutRef<T>(ref T reference) : IQueryParam<OutRef<T>>
{
    private readonly ref T _reference = ref reference;

    /// <summary>
    /// Sets the value of the reference.
    /// </summary>
    public void SetValue(T value) => _reference = value;

    static QueryParamGlobalCache IQueryParam<OutRef<T>>.GetGlobalCache(World world)
    {
        var idCache = QueryParam.GetCache<StrongBox<Identifier>>();
        idCache.Value = world.GetEntityForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<OutRef<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

    static QueryParamArchetypeCache IQueryParam<OutRef<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;
        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static OutRef<T> IQueryParam<OutRef<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        var collection = QueryParam.UnsafeCastCache<IComponentCollection<T>>(entry.ArchetypeCache);
        return new(ref collection[entry.EntityIndex]!);
    }

    static void IQueryParam<OutRef<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
