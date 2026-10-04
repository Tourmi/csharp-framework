namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

internal class ComponentEntityOverride : IQueryParamOverride<ComponentEntity>
{
    static QueryParamGlobalCache IQueryParamOverride<ComponentEntity>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParamOverride<ComponentEntity>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static ComponentEntity IQueryParamOverride<ComponentEntity>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex], QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache));

    static void IQueryParamOverride<ComponentEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<Component>();
}
