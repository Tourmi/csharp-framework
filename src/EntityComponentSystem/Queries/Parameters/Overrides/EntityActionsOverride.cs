namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

internal class EntityActionsOverride : IQueryParamOverride<EntityActions>
{
    static QueryParamGlobalCache IQueryParamOverride<EntityActions>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParamOverride<EntityActions>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static EntityActions IQueryParamOverride<EntityActions>.CreateFrom(QueryParamEntityInfo entry)
        => QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache);
}
