using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query Parameter that marks the component <typeparamref name="T"/> as required, 
/// without including its value.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Needed for terse queries.")]
public readonly ref struct Optional<T>() : IQueryParam<Optional<T>>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    /// <summary>
    /// Constructs an optional that has a value.
    /// </summary>
    public Optional(T value) : this()
    {
        _value = value;
        _hasValue = true;
    }

    /// <summary>
    /// Returns <see langword="true"/> if the parameter exists in the entity.
    /// </summary>
    [MemberNotNullWhen(true, nameof(ValueOrDefault))]
    public bool HasValue => _hasValue;

    /// <summary>
    /// Returns the optional param's value.
    /// </summary>
    public T Value => _hasValue ? _value! : throw new InvalidOperationException("Optional does not have a value.");

    /// <summary>
    /// Returns the value, or <see langword="default"/> if the optional param doesn't have a value.
    /// </summary>
    public T? ValueOrDefault => _value;

    static QueryParamGlobalCache IQueryParam<Optional<T>>.GetGlobalCache(World world)
    {
        var idCache = QueryParam.GetCache<StrongBox<Identifier>>();
        idCache.Value = world.GetComponentForType<T>().Id;
        return new(idCache);
    }

    static void IQueryParam<Optional<T>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

    static QueryParamArchetypeCache IQueryParam<Optional<T>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;

        if (!archetype.HasComponent(componentId))
        {
            return new(null);
        }

        return new(archetype.GetComponentCollection<T>(componentId));
    }

    static Optional<T> IQueryParam<Optional<T>>.CreateFrom(QueryParamEntityInfo entry)
    {
        // If the cache doesn't exist, then the component doesn't exist in the entity.
        if (entry.ArchetypeCache.Value is null)
        {
            return default;
        }

        var collection = QueryParam.UnsafeCastCache<IComponentCollection<T>>(entry.ArchetypeCache);
        return new(collection[entry.EntityIndex]!);
    }
}
