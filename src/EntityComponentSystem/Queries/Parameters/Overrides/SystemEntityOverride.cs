namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

internal class SystemEntityOverride : IQueryParamOverride<SystemEntity>
{
    static QueryParamGlobalCache IQueryParamOverride<SystemEntity>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParamOverride<SystemEntity>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static SystemEntity IQueryParamOverride<SystemEntity>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex], QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache));

    static void IQueryParamOverride<SystemEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<SystemComponent>();
}
