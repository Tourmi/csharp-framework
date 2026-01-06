using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Type that only exists because C# Default interface implementations suck.
/// </summary>
internal readonly struct DummyQueryParam : IQueryParam<DummyQueryParam>
{
    static QueryParamGlobalCache IQueryParam<DummyQueryParam>.GetGlobalCache(World world) => default;

    static void IQueryParam<DummyQueryParam>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world) { }

    static QueryParamArchetypeCache IQueryParam<DummyQueryParam>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache) => default;

    static void IQueryParam<DummyQueryParam>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype) { }

    static DummyQueryParam IQueryParam<DummyQueryParam>.CreateFrom(QueryParamEntityInfo entry) => default;
}