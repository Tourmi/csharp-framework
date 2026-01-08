using System.Diagnostics;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;
using Tourmi.Framework.Collections;
using Tourmi.Framework.Runtime;

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
        var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
        if (!pool.TryTake(out var idCache))
        {
            idCache = new();
        }

        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<ThreadSafe<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

        var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
        pool.Return(Unsafe.As<StrongBox<Identifier>>(existingCache.Value));
    }

    static QueryParamArchetypeCache IQueryParam<ThreadSafe<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

        var componentId = Unsafe.As<StrongBox<Identifier>>(globalCache.Value).Value;

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static ThreadSafe<T> IQueryParam<ThreadSafe<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is IComponentCollection<T>, "Given cache was of the wrong type.");
        var collection = Unsafe.As<IComponentCollection<T>>(entry.ArchetypeCache.Value);
        return new(collection[entry.EntityIndex]!);
    }

    static void IQueryParam<ThreadSafe<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
