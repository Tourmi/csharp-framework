namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

internal class EntityOverride : IQueryParamOverride<Entity>
{
    static QueryParamGlobalCache IQueryParamOverride<Entity>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParamOverride<Entity>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static Entity IQueryParamOverride<Entity>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex], QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache));
}
