using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Contains callbacks to the functions that <typeparamref name="T"/> uses to populate queries.
/// </summary>
internal readonly record struct ParamCallbacks<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>
    where T : allows ref struct
{
    static ParamCallbacks()
    {
        Instance = GetCallbacks();
    }

    private ParamCallbacks(
        Func<World, QueryParamGlobalCache> getGlobalCache,
        Action<QueryParamGlobalCache, World> freeGlobalCache,
        Func<World, Archetype, QueryParamGlobalCache, QueryParamArchetypeCache> getArchetypeCache,
        Action<QueryParamArchetypeCache, QueryParamGlobalCache, World, Archetype> freeArchetypeCache,
        Func<QueryParamEntityInfo, T> createFrom,
        Action<EntityFilter> updateFilter)
    {
        GetGlobalCache = getGlobalCache;
        FreeGlobalCache = freeGlobalCache;
        GetArchetypeCache = getArchetypeCache;
        FreeArchetypeCache = freeArchetypeCache;
        CreateFrom = createFrom;
        UpdateFilter = updateFilter;
    }

    public static ParamCallbacks<T> Instance { get; }

    public Func<World, QueryParamGlobalCache> GetGlobalCache { get; }
    public Action<QueryParamGlobalCache, World> FreeGlobalCache { get; }
    public Func<World, Archetype, QueryParamGlobalCache, QueryParamArchetypeCache> GetArchetypeCache { get; }
    public Action<QueryParamArchetypeCache, QueryParamGlobalCache, World, Archetype> FreeArchetypeCache { get; }
    public Func<QueryParamEntityInfo, T> CreateFrom { get; }
    public Action<EntityFilter> UpdateFilter { get; }

    internal static ParamCallbacks<T> GetCallbacks()
    {
        var interfaceTypes = typeof(T).GetInterfaces();
        var index = interfaceTypes.Index()
            .Where(i => i.Item.IsGenericType && i.Item.GetGenericTypeDefinition() == typeof(IQueryParam<>))
            .Select(i => i.Index)
            .DefaultIfEmpty(-1)
            .FirstOrDefault();
        if (index < 0)
        {
            if (typeof(T).IsByRefLike || typeof(T).IsByRef)
            {
                throw new InvalidOperationException("Refs and ByRefLike types are not supported for custom types");
            }

            var callback = typeof(ParamCallbacks<T>)
                .GetMethod(nameof(GetDefaultCallbacksFor), BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!
                .MakeGenericMethod(typeof(T))
                .CreateDelegate<Func<ParamCallbacks<T>>>();

            return callback();
        }

        var interfaceType = interfaceTypes[index];
#pragma warning disable IL2062 // The parameter of method has a DynamicallyAccessedMembersAttribute, but the value passed to it can not be statically analyzed.
        var mapping = typeof(T).GetInterfaceMap(interfaceType)!;
#pragma warning restore IL2062

        var getGlobalCache = GetDelegate<Func<World, QueryParamGlobalCache>>(mapping, nameof(IQueryParam<>.GetGlobalCache));
        var freeGlobalCache = GetDelegate<Action<QueryParamGlobalCache, World>>(mapping, nameof(IQueryParam<>.FreeGlobalCache));
        var getArchetypeCache = GetDelegate<Func<World, Archetype, QueryParamGlobalCache, QueryParamArchetypeCache>>(mapping, nameof(IQueryParam<>.GetArchetypeCache));
        var freeArchetypeCache = GetDelegate<Action<QueryParamArchetypeCache, QueryParamGlobalCache, World, Archetype>>(mapping, nameof(IQueryParam<>.FreeArchetypeCache));
        var createFrom = GetDelegate<Func<QueryParamEntityInfo, T>>(mapping, nameof(IQueryParam<>.CreateFrom));
        var updateFilter = GetDelegate<Action<EntityFilter>>(mapping, nameof(IQueryParam<>.UpdateFilter));

        return new(getGlobalCache, freeGlobalCache, getArchetypeCache, freeArchetypeCache, createFrom, updateFilter);

        static TDelegate GetDelegate<TDelegate>(InterfaceMapping mapping, string methodName) where TDelegate : Delegate
        {
            var targetMethod = mapping.TargetMethods[mapping
                    .InterfaceMethods
                    .Index()
                    .First(m => m.Item.Name == methodName)
                    .Index];

            // If the target method is virtual, it means that it is not implemented by the type,
            // so fall back to the dummy/no-op implementation.
            if (targetMethod.IsVirtual)
            {
                var interfaceType = typeof(DummyQueryParam)
                    .GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryParam<>))
                    .First();
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
                var newMapping = typeof(DummyQueryParam).GetInterfaceMap(interfaceType);
#pragma warning restore IL2072

                return GetDelegate<TDelegate>(newMapping, methodName);
            }

            return targetMethod.CreateDelegate<TDelegate>();
        }
    }

    private static ParamCallbacks<TOther> GetDefaultCallbacksFor<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] TOther>()
    {
        return new(GetGlobalCache, FreeGlobalCache, GetArchetypeCache, FreeArchetypeCache, CreateFrom, UpdateFilter);

        static QueryParamGlobalCache GetGlobalCache(World world)
        {
            var idCache = QueryParam.GetCache<StrongBox<Identifier>>();
            idCache.Value = world.GetComponentForType<TOther>().Id;
            return new(idCache);
        }

        static void FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
            => QueryParam.FreeCache(QueryParam.UnsafeCastCache<StrongBox<Identifier>>(existingCache));

        static QueryParamArchetypeCache GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
        {
            var componentId = QueryParam.UnsafeCastCache<StrongBox<Identifier>>(globalCache).Value;

            return new(archetype.GetComponentCollection<TOther>(componentId));
        }

        static void FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
        {
            // Nothing to free
        }

        static TOther CreateFrom(QueryParamEntityInfo entry)
        {
            var collection = QueryParam.UnsafeCastCache<IComponentCollection<TOther>>(entry.ArchetypeCache);
            return collection[entry.EntityIndex]!;
        }

        static void UpdateFilter(EntityFilter filter) => filter.Requires<TOther>();
    }
}
