namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

/// <summary>
/// Defines query param overrides for a type <typeparamref name="T"/> that doesn't implement the <see cref="IQueryParam{T}"/> interface.
/// </summary>
internal interface IQueryParamOverride<T> : IQueryParamOverride
    where T : allows ref struct
{
    /// <inheritdoc cref="IQueryParam{T}.GetGlobalCache(World)"/>
    internal static virtual QueryParamGlobalCache GetGlobalCache(World world) => default;

    /// <inheritdoc cref="IQueryParam{T}.FreeGlobalCache"/>
    internal static virtual void FreeGlobalCache(
        QueryParamGlobalCache cacheToFree,
        World world)
    {
    }

    /// <inheritdoc cref="IQueryParam{T}.GetArchetypeCache"/>
    internal static virtual QueryParamArchetypeCache GetArchetypeCache(
        World world,
        Archetype archetype,
        QueryParamGlobalCache globalCache) => default;

    /// <inheritdoc cref="IQueryParam{T}.FreeArchetypeCache"/>
    internal static virtual void FreeArchetypeCache(
        QueryParamArchetypeCache cacheToFree,
        QueryParamGlobalCache globalCache,
        World world,
        Archetype archetype)
    {
    }

    /// <inheritdoc cref="IQueryParam{T}.CreateFrom"/>
    internal static abstract T CreateFrom(QueryParamEntityInfo entry);

    /// <inheritdoc cref="IQueryParam{T}.UpdateFilter"/>
    internal static virtual void UpdateFilter(EntityFilter filter) { }
}
