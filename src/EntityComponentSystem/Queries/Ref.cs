using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;
using Tourmi.Framework.Collections;
using Tourmi.Framework.Runtime;

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
        var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
        if (!pool.TryTake(out var idCache))
        {
            idCache = new();
        }

        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<Ref<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

        var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
        pool.Return((StrongBox<Identifier>)existingCache.Value!);
    }


    static QueryParamArchetypeCache IQueryParam<Ref<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

        var componentId = ((StrongBox<Identifier>)globalCache.Value)!.Value;

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static Ref<T> IQueryParam<Ref<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is IComponentCollection<T>, "Given cache was of the wrong type.");
        var collection = (IComponentCollection<T>)entry.ArchetypeCache.Value!;
        return new(ref collection[entry.EntityIndex]!);
    }
}
