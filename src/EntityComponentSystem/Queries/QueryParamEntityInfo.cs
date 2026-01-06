using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Information given to fetch information for populating a query parameter for an entity
/// </summary>
internal readonly record struct QueryParamEntityInfo(
    World World, 
    Archetype Archetype, 
    int EntityIndex, 
    QueryParamGlobalCache GlobalCache, 
    QueryParamArchetypeCache ArchetypeCache);
