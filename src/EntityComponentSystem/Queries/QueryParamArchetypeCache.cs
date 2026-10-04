namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Cache that applies to all entities within an archetype
/// </summary>
internal readonly record struct QueryParamArchetypeCache(object? Value);
