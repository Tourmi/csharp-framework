namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Cache that applies to the entire iteration of a query.
/// </summary>
internal readonly record struct QueryParamGlobalCache(object? Value);
