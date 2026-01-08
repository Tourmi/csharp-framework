using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;
using Tourmi.Framework.Collections;
using Tourmi.Framework.Runtime;

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
        var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
        if (!pool.TryTake(out var idCache))
        {
            idCache = new();
        }

        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<RefReadonly<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

        var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
        pool.Return(Unsafe.As<StrongBox<Identifier>>(existingCache.Value!));
    }

    static QueryParamArchetypeCache IQueryParam<RefReadonly<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

        var componentId = Unsafe.As<StrongBox<Identifier>>((StrongBox<Identifier>)globalCache.Value).Value;

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static RefReadonly<T> IQueryParam<RefReadonly<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is IComponentCollection<T>, "Given cache was of the wrong type.");
        var collection = Unsafe.As<IComponentCollection<T>>(entry.ArchetypeCache.Value);
        return new(ref collection[entry.EntityIndex]!);
    }

    static void IQueryParam<RefReadonly<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
