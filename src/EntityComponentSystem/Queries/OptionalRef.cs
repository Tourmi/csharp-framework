using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query Parameter that contains a reference to the component <typeparamref name="T"/>, 
/// without it being required.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Needed for terse queries.")]
public readonly ref struct OptionalRef<T>() : IQueryParam<OptionalRef<T>>
{
    private readonly ref T? _value;
    private readonly bool _hasValue;

    /// <summary>
    /// Constructs an optional that has a value.
    /// </summary>
    public OptionalRef(ref T value) : this()
    {
        _value = ref value!;
        _hasValue = true;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the parameter exists in the entity.
    /// </summary>
    public bool HasValue => _hasValue;

    /// <summary>
    /// Returns the optional param's reference.
    /// </summary>
    public ref T Reference
    {
        get
        {
            if (!_hasValue)
            {
                throw new InvalidOperationException("Optional does not have a value.");
            }

            return ref _value!;
        }
    }

    static QueryParamGlobalCache IQueryParam<OptionalRef<T>>.GetGlobalCache(World world)
    {
        var idCache = QueryParam.GetCache<StrongBox<Identifier>>();
        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<OptionalRef<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

    static QueryParamArchetypeCache IQueryParam<OptionalRef<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;

        if (!archetype.HasComponent(componentId))
        {
            return new(null);
        }

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static OptionalRef<T> IQueryParam<OptionalRef<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        // If the cache doesn't exist, then the component doesn't exist in the entity.
        if (entry.ArchetypeCache.Value is null)
        {
            return default;
        }

        var collection = QueryParam.UnsafeCastCache<IComponentCollection<T>>(entry.ArchetypeCache);
        return new(ref collection[entry.EntityIndex]!);
    }
}
