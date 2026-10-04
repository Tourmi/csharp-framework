namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

internal class PrefabEntityOverride : IQueryParamOverride<PrefabEntity>
{
    static QueryParamGlobalCache IQueryParamOverride<PrefabEntity>.GetGlobalCache(World world) => new(world.GetEntityActions());

    static void IQueryParamOverride<PrefabEntity>.FreeGlobalCache(QueryParamGlobalCache cacheToFree, World world)
        => world.ReturnQueuedActions(QueryParam.UnsafeCastCache<EntityActions>(cacheToFree));

    static PrefabEntity IQueryParamOverride<PrefabEntity>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex], QueryParam.UnsafeCastCache<EntityActions>(entry.GlobalCache));

    static void IQueryParamOverride<PrefabEntity>.UpdateFilter(EntityFilter filter) => filter.Requires<Prefab>();
}
