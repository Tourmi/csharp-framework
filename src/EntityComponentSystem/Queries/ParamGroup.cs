using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;
using Tourmi.Framework.Collections;
using Tourmi.Framework.Runtime;

using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;
using static Tourmi.EntityComponentSystem.Queries.ParamGroup;

using ArchetypeCache1 = System.Runtime.CompilerServices.StrongBox<
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache>;
using ArchetypeCache2 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using ArchetypeCache3 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using ArchetypeCache4 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using ArchetypeCache5 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using ArchetypeCache6 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using ArchetypeCache7 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using ArchetypeCache8 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamArchetypeCache)>;
using GlobalCache1 = System.Runtime.CompilerServices.StrongBox<
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache>;
using GlobalCache2 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;
using GlobalCache3 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;
using GlobalCache4 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;
using GlobalCache5 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;
using GlobalCache6 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;
using GlobalCache7 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;
using GlobalCache8 = System.Runtime.CompilerServices.StrongBox<(
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache,
    Tourmi.EntityComponentSystem.Queries.QueryParamGlobalCache)>;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Wrapper type for multiple query parameters.
/// Can be used to group parameters of a query together for better legibility, or when a <see cref="Query"/> has too many parameters.
/// </summary>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T8>(
        T1 param1,
        T2 param2,
        T3 param3,
        T4 param4,
        T5 param5,
        T6 param6,
        T7 param7,
        T8 param8) : IParamGroup, IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
    where T8 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();
    private static readonly ParamCallbacks<T3> Value3Callbacks = GetCallbacksFor<T3>();
    private static readonly ParamCallbacks<T4> Value4Callbacks = GetCallbacksFor<T4>();
    private static readonly ParamCallbacks<T5> Value5Callbacks = GetCallbacksFor<T5>();
    private static readonly ParamCallbacks<T6> Value6Callbacks = GetCallbacksFor<T6>();
    private static readonly ParamCallbacks<T7> Value7Callbacks = GetCallbacksFor<T7>();
    private static readonly ParamCallbacks<T8> Value8Callbacks = GetCallbacksFor<T8>();

    /// <summary>
    /// First value of the parameter group.
    /// </summary>
    public T1 Value1 { get; } = param1;

    /// <summary>
    /// Second value of the parameter group.
    /// </summary>
    public T2 Value2 { get; } = param2;

    /// <summary>
    /// Third value of the parameter group.
    /// </summary>
    public T3 Value3 { get; } = param3;

    /// <summary>
    /// Fourth value of the parameter group.
    /// </summary>
    public T4 Value4 { get; } = param4;

    /// <summary>
    /// Fifth value of the parameter group.
    /// </summary>
    public T5 Value5 { get; } = param5;

    /// <summary>
    /// Sixth value of the parameter group.
    /// </summary>
    public T6 Value6 { get; } = param6;

    /// <summary>
    /// Seventh value of the parameter group.
    /// </summary>
    public T7 Value7 { get; } = param7;

    /// <summary>
    /// Eighth value of the parameter group.
    /// </summary>
    public T8 Value8 { get; } = param8;

    /// <inheritdoc/>
    public int Count => 8;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache8>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world),
            Value3Callbacks.GetGlobalCache(world),
            Value4Callbacks.GetGlobalCache(world),
            Value5Callbacks.GetGlobalCache(world),
            Value6Callbacks.GetGlobalCache(world),
            Value7Callbacks.GetGlobalCache(world),
            Value8Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache8, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache8>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);
        Value3Callbacks.FreeGlobalCache(caches.Value.Item3, world);
        Value4Callbacks.FreeGlobalCache(caches.Value.Item4, world);
        Value5Callbacks.FreeGlobalCache(caches.Value.Item5, world);
        Value6Callbacks.FreeGlobalCache(caches.Value.Item6, world);
        Value7Callbacks.FreeGlobalCache(caches.Value.Item7, world);
        Value8Callbacks.FreeGlobalCache(caches.Value.Item8, world);

        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache8, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache8>(globalCache.Value);

        var cache = GetCache<ArchetypeCache8>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2),
            Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item3),
            Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item4),
            Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item5),
            Value6Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item6),
            Value7Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item7),
            Value8Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item8));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache8, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache8, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache8>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache8>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);
        Value3Callbacks.FreeArchetypeCache(caches.Value.Item3, globalCaches.Value.Item3, world, archetype);
        Value4Callbacks.FreeArchetypeCache(caches.Value.Item4, globalCaches.Value.Item4, world, archetype);
        Value5Callbacks.FreeArchetypeCache(caches.Value.Item5, globalCaches.Value.Item5, world, archetype);
        Value6Callbacks.FreeArchetypeCache(caches.Value.Item6, globalCaches.Value.Item6, world, archetype);
        Value7Callbacks.FreeArchetypeCache(caches.Value.Item7, globalCaches.Value.Item7, world, archetype);
        Value8Callbacks.FreeArchetypeCache(caches.Value.Item8, globalCaches.Value.Item8, world, archetype);

        FreeCache(caches);
    }

    static ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8> IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache8, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache8, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache8>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache8>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item3, ArchetypeCache = caches.Value.Item3 });
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item4, ArchetypeCache = caches.Value.Item4 });
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item5, ArchetypeCache = caches.Value.Item5 });
        var val6 = Value6Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item6, ArchetypeCache = caches.Value.Item6 });
        var val7 = Value7Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item7, ArchetypeCache = caches.Value.Item7 });
        var val8 = Value8Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item8, ArchetypeCache = caches.Value.Item8 });

        return new(val1, val2, val3, val4, val5, val6, val7, val8);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
        Value3Callbacks.UpdateFilter(filter);
        Value4Callbacks.UpdateFilter(filter);
        Value5Callbacks.UpdateFilter(filter);
        Value6Callbacks.UpdateFilter(filter);
        Value7Callbacks.UpdateFilter(filter);
        Value8Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7>(
        T1 param1,
        T2 param2,
        T3 param3,
        T4 param4,
        T5 param5,
        T6 param6,
        T7 param7) : IParamGroup, IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();
    private static readonly ParamCallbacks<T3> Value3Callbacks = GetCallbacksFor<T3>();
    private static readonly ParamCallbacks<T4> Value4Callbacks = GetCallbacksFor<T4>();
    private static readonly ParamCallbacks<T5> Value5Callbacks = GetCallbacksFor<T5>();
    private static readonly ParamCallbacks<T6> Value6Callbacks = GetCallbacksFor<T6>();
    private static readonly ParamCallbacks<T7> Value7Callbacks = GetCallbacksFor<T7>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value5"/>
    public T5 Value5 { get; } = param5;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value6"/>
    public T6 Value6 { get; } = param6;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value7"/>
    public T7 Value7 { get; } = param7;

    /// <inheritdoc/>
    public int Count => 7;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache7>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world),
            Value3Callbacks.GetGlobalCache(world),
            Value4Callbacks.GetGlobalCache(world),
            Value5Callbacks.GetGlobalCache(world),
            Value6Callbacks.GetGlobalCache(world),
            Value7Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache7, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache7>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);
        Value3Callbacks.FreeGlobalCache(caches.Value.Item3, world);
        Value4Callbacks.FreeGlobalCache(caches.Value.Item4, world);
        Value5Callbacks.FreeGlobalCache(caches.Value.Item5, world);
        Value6Callbacks.FreeGlobalCache(caches.Value.Item6, world);
        Value7Callbacks.FreeGlobalCache(caches.Value.Item7, world);

        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache7, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache7>(globalCache.Value);

        var cache = GetCache<ArchetypeCache7>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2),
            Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item3),
            Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item4),
            Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item5),
            Value6Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item6),
            Value7Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item7));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache7, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache7, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache7>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache7>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);
        Value3Callbacks.FreeArchetypeCache(caches.Value.Item3, globalCaches.Value.Item3, world, archetype);
        Value4Callbacks.FreeArchetypeCache(caches.Value.Item4, globalCaches.Value.Item4, world, archetype);
        Value5Callbacks.FreeArchetypeCache(caches.Value.Item5, globalCaches.Value.Item5, world, archetype);
        Value6Callbacks.FreeArchetypeCache(caches.Value.Item6, globalCaches.Value.Item6, world, archetype);
        Value7Callbacks.FreeArchetypeCache(caches.Value.Item7, globalCaches.Value.Item7, world, archetype);

        FreeCache(caches);
    }

    static ParamGroup<T1, T2, T3, T4, T5, T6, T7> IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache7, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache7, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache7>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache7>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item3, ArchetypeCache = caches.Value.Item3 });
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item4, ArchetypeCache = caches.Value.Item4 });
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item5, ArchetypeCache = caches.Value.Item5 });
        var val6 = Value6Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item6, ArchetypeCache = caches.Value.Item6 });
        var val7 = Value7Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item7, ArchetypeCache = caches.Value.Item7 });

        return new(val1, val2, val3, val4, val5, val6, val7);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
        Value3Callbacks.UpdateFilter(filter);
        Value4Callbacks.UpdateFilter(filter);
        Value5Callbacks.UpdateFilter(filter);
        Value6Callbacks.UpdateFilter(filter);
        Value7Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6>(
        T1 param1,
        T2 param2,
        T3 param3,
        T4 param4,
        T5 param5,
        T6 param6) : IParamGroup, IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();
    private static readonly ParamCallbacks<T3> Value3Callbacks = GetCallbacksFor<T3>();
    private static readonly ParamCallbacks<T4> Value4Callbacks = GetCallbacksFor<T4>();
    private static readonly ParamCallbacks<T5> Value5Callbacks = GetCallbacksFor<T5>();
    private static readonly ParamCallbacks<T6> Value6Callbacks = GetCallbacksFor<T6>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value5"/>
    public T5 Value5 { get; } = param5;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value6"/>
    public T6 Value6 { get; } = param6;

    /// <inheritdoc/>
    public int Count => 6;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache6>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world),
            Value3Callbacks.GetGlobalCache(world),
            Value4Callbacks.GetGlobalCache(world),
            Value5Callbacks.GetGlobalCache(world),
            Value6Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache6, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache6>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);
        Value3Callbacks.FreeGlobalCache(caches.Value.Item3, world);
        Value4Callbacks.FreeGlobalCache(caches.Value.Item4, world);
        Value5Callbacks.FreeGlobalCache(caches.Value.Item5, world);
        Value6Callbacks.FreeGlobalCache(caches.Value.Item6, world);

        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache6, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache6>(globalCache.Value);

        var cache = GetCache<ArchetypeCache6>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2),
            Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item3),
            Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item4),
            Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item5),
            Value6Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item6));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache6, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache6, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache6>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache6>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);
        Value3Callbacks.FreeArchetypeCache(caches.Value.Item3, globalCaches.Value.Item3, world, archetype);
        Value4Callbacks.FreeArchetypeCache(caches.Value.Item4, globalCaches.Value.Item4, world, archetype);
        Value5Callbacks.FreeArchetypeCache(caches.Value.Item5, globalCaches.Value.Item5, world, archetype);
        Value6Callbacks.FreeArchetypeCache(caches.Value.Item6, globalCaches.Value.Item6, world, archetype);

        FreeCache(caches);
    }

    static ParamGroup<T1, T2, T3, T4, T5, T6> IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache6, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache6, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache6>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache6>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item3, ArchetypeCache = caches.Value.Item3 });
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item4, ArchetypeCache = caches.Value.Item4 });
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item5, ArchetypeCache = caches.Value.Item5 });
        var val6 = Value6Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item6, ArchetypeCache = caches.Value.Item6 });

        return new(val1, val2, val3, val4, val5, val6);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
        Value3Callbacks.UpdateFilter(filter);
        Value4Callbacks.UpdateFilter(filter);
        Value5Callbacks.UpdateFilter(filter);
        Value6Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5>(
        T1 param1,
        T2 param2,
        T3 param3,
        T4 param4,
        T5 param5) : IParamGroup, IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();
    private static readonly ParamCallbacks<T3> Value3Callbacks = GetCallbacksFor<T3>();
    private static readonly ParamCallbacks<T4> Value4Callbacks = GetCallbacksFor<T4>();
    private static readonly ParamCallbacks<T5> Value5Callbacks = GetCallbacksFor<T5>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value5"/>
    public T5 Value5 { get; } = param5;

    /// <inheritdoc/>
    public int Count => 5;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache5>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world),
            Value3Callbacks.GetGlobalCache(world),
            Value4Callbacks.GetGlobalCache(world),
            Value5Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache5, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache5>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);
        Value3Callbacks.FreeGlobalCache(caches.Value.Item3, world);
        Value4Callbacks.FreeGlobalCache(caches.Value.Item4, world);
        Value5Callbacks.FreeGlobalCache(caches.Value.Item5, world);

        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache5, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache5>(globalCache.Value);

        var cache = GetCache<ArchetypeCache5>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2),
            Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item3),
            Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item4),
            Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item5));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache5, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache5, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache5>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache5>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);
        Value3Callbacks.FreeArchetypeCache(caches.Value.Item3, globalCaches.Value.Item3, world, archetype);
        Value4Callbacks.FreeArchetypeCache(caches.Value.Item4, globalCaches.Value.Item4, world, archetype);
        Value5Callbacks.FreeArchetypeCache(caches.Value.Item5, globalCaches.Value.Item5, world, archetype);

        FreeCache(caches);
    }

    static ParamGroup<T1, T2, T3, T4, T5> IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache5, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache5, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache5>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache5>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item3, ArchetypeCache = caches.Value.Item3 });
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item4, ArchetypeCache = caches.Value.Item4 });
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item5, ArchetypeCache = caches.Value.Item5 });

        return new(val1, val2, val3, val4, val5);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
        Value3Callbacks.UpdateFilter(filter);
        Value4Callbacks.UpdateFilter(filter);
        Value5Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4>(
        T1 param1,
        T2 param2,
        T3 param3,
        T4 param4) : IParamGroup, IQueryParam<ParamGroup<T1, T2, T3, T4>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();
    private static readonly ParamCallbacks<T3> Value3Callbacks = GetCallbacksFor<T3>();
    private static readonly ParamCallbacks<T4> Value4Callbacks = GetCallbacksFor<T4>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc/>
    public int Count => 4;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2, T3, T4>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache4>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world),
            Value3Callbacks.GetGlobalCache(world),
            Value4Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache4, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache4>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);
        Value3Callbacks.FreeGlobalCache(caches.Value.Item3, world);
        Value4Callbacks.FreeGlobalCache(caches.Value.Item4, world);

        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache4, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache4>(globalCache.Value);

        var cache = GetCache<ArchetypeCache4>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2),
            Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item3),
            Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item4));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache4, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache4, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache4>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache4>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);
        Value3Callbacks.FreeArchetypeCache(caches.Value.Item3, globalCaches.Value.Item3, world, archetype);
        Value4Callbacks.FreeArchetypeCache(caches.Value.Item4, globalCaches.Value.Item4, world, archetype);

        FreeCache(caches);
    }

    static ParamGroup<T1, T2, T3, T4> IQueryParam<ParamGroup<T1, T2, T3, T4>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache4, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache4, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache4>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache4>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item3, ArchetypeCache = caches.Value.Item3 });
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item4, ArchetypeCache = caches.Value.Item4 });

        return new(val1, val2, val3, val4);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
        Value3Callbacks.UpdateFilter(filter);
        Value4Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3>(
        T1 param1,
        T2 param2,
        T3 param3) : IParamGroup, IQueryParam<ParamGroup<T1, T2, T3>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();
    private static readonly ParamCallbacks<T3> Value3Callbacks = GetCallbacksFor<T3>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc/>
    public int Count => 3;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2, T3>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache3>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world),
            Value3Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache3, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache3>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);
        Value3Callbacks.FreeGlobalCache(caches.Value.Item3, world);

        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache3, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache3>(globalCache.Value);

        var cache = GetCache<ArchetypeCache3>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2),
            Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item3));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache3, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache3, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache3>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache3>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);
        Value3Callbacks.FreeArchetypeCache(caches.Value.Item3, globalCaches.Value.Item3, world, archetype);


        FreeCache(caches);
    }

    static ParamGroup<T1, T2, T3> IQueryParam<ParamGroup<T1, T2, T3>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache3, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache3, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache3>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache3>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item3, ArchetypeCache = caches.Value.Item3 });

        return new(val1, val2, val3);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
        Value3Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
    [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2>(
        T1 param1,
        T2 param2) : IParamGroup, IQueryParam<ParamGroup<T1, T2>>
    where T1 : allows ref struct
    where T2 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();
    private static readonly ParamCallbacks<T2> Value2Callbacks = GetCallbacksFor<T2>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc/>
    public int Count => 2;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1, T2>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache2>();

        cache.Value = (
            Value1Callbacks.GetGlobalCache(world),
            Value2Callbacks.GetGlobalCache(world));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache2, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache2>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value.Item1, world);
        Value2Callbacks.FreeGlobalCache(caches.Value.Item2, world);


        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache2, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache2>(globalCache.Value);

        var cache = GetCache<ArchetypeCache2>();

        cache.Value = (
            Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item1),
            Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value.Item2));

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1, T2>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache2, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache2, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache2>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache2>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value.Item1, globalCaches.Value.Item1, world, archetype);
        Value2Callbacks.FreeArchetypeCache(caches.Value.Item2, globalCaches.Value.Item2, world, archetype);


        FreeCache(caches);
    }

    static ParamGroup<T1, T2> IQueryParam<ParamGroup<T1, T2>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache2, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache2, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache2>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache2>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item1, ArchetypeCache = caches.Value.Item1 });
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value.Item2, ArchetypeCache = caches.Value.Item2 });

        return new(val1, val2);
    }

    static void IQueryParam<ParamGroup<T1, T2>>.UpdateFilter(EntityFilter filter)
    {
        Value1Callbacks.UpdateFilter(filter);
        Value2Callbacks.UpdateFilter(filter);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1>(
        T1 param1) : IParamGroup, IQueryParam<ParamGroup<T1>>
    where T1 : allows ref struct
{
    private static readonly ParamCallbacks<T1> Value1Callbacks = GetCallbacksFor<T1>();

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc/>
    public int Count => 1;

    static QueryParamGlobalCache IQueryParam<ParamGroup<T1>>.GetGlobalCache(World world)
    {
        var cache = GetCache<GlobalCache1>();

        cache.Value = Value1Callbacks.GetGlobalCache(world);

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is GlobalCache1, "Cache didn't have expected type.");

        var caches = Unsafe.As<GlobalCache1>(existingCache.Value);
        Value1Callbacks.FreeGlobalCache(caches.Value, world);


        FreeCache(caches);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is GlobalCache1, "Cache didn't have expected type.");
        var globalCaches = Unsafe.As<GlobalCache1>(globalCache.Value);

        var cache = GetCache<ArchetypeCache1>();

        cache.Value = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches.Value);

        return new(cache);
    }

    static void IQueryParam<ParamGroup<T1>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is ArchetypeCache1, "Cache didn't have expected type.");
        Debug.Assert(globalCache.Value is GlobalCache1, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache1>(existingCache.Value);
        var globalCaches = Unsafe.As<GlobalCache1>(globalCache.Value);
        Value1Callbacks.FreeArchetypeCache(caches.Value, globalCaches.Value, world, archetype);

        FreeCache(caches);
    }

    static ParamGroup<T1> IQueryParam<ParamGroup<T1>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is ArchetypeCache1, "Cache didn't have expected type.");
        Debug.Assert(entry.GlobalCache.Value is GlobalCache1, "Cache didn't have expected type.");

        var caches = Unsafe.As<ArchetypeCache1>(entry.ArchetypeCache.Value);
        var globalCaches = Unsafe.As<GlobalCache1>(entry.GlobalCache.Value);

        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches.Value, ArchetypeCache = caches.Value });

        return new(val1);
    }

    static void IQueryParam<ParamGroup<T1>>.UpdateFilter(EntityFilter filter) => Value1Callbacks.UpdateFilter(filter);
}

/// <summary>
/// Special Parameter Group containing no parameters.
/// </summary>
public readonly ref struct ParamGroup() : IParamGroup, IQueryParam<ParamGroup>
{
    internal record ParamCallbacks<T>(
        Func<World, QueryParamGlobalCache> GetGlobalCache,
        Action<QueryParamGlobalCache, World> FreeGlobalCache,
        Func<World, Archetype, QueryParamGlobalCache, QueryParamArchetypeCache> GetArchetypeCache,
        Action<QueryParamArchetypeCache, QueryParamGlobalCache, World, Archetype> FreeArchetypeCache,
        Func<QueryParamEntityInfo, T> CreateFrom,
        Action<EntityFilter> UpdateFilter
    ) where T : allows ref struct;

    /// <inheritdoc/>
    public int Count => 0;

    internal static T GetCache<T>()
        where T : class, new()
    {
        var pool = ThreadStaticProvider<Pool<T>>.Value;
        if (!pool.TryTake(out var cache))
        {
            cache = new();
        }

        return cache;
    }

    internal static void FreeCache<T>(T cache)
        where T : class, new()
    {
        var pool = ThreadStaticProvider<Pool<T>>.Value;
        pool.Return(cache);
    }

    internal static ParamCallbacks<T> GetCallbacksFor<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>()

        where T : allows ref struct
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

            var callback = typeof(ParamGroup)
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

    private static ParamCallbacks<T> GetDefaultCallbacksFor<T>()
    {
        return new(GetGlobalCache, FreeGlobalCache, GetArchetypeCache, FreeArchetypeCache, CreateFrom, UpdateFilter);

        static QueryParamGlobalCache GetGlobalCache(World world)
        {
            var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
            if (!pool.TryTake(out var idCache))
            {
                idCache = new();
            }

            idCache.Value = world.GetComponentForType<T>().Id;
            return new(idCache);
        }

        static void FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
        {
            Debug.Assert(existingCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

            var pool = ThreadStaticProvider<Pool<StrongBox<Identifier>>>.Value;
            pool.Return(Unsafe.As<StrongBox<Identifier>>(existingCache.Value));
        }

        static QueryParamArchetypeCache GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
        {
            Debug.Assert(globalCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

            var componentId = Unsafe.As<StrongBox<Identifier>>(globalCache.Value).Value;

            return new(archetype.GetComponentCollection<T>(componentId));
        }

        static void FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
        {
            // Nothing to free
        }

        static T CreateFrom(QueryParamEntityInfo entry)
        {
            Debug.Assert(entry.ArchetypeCache.Value is IComponentCollection<T>, "Given cache was of the wrong type.");
            var collection = Unsafe.As<IComponentCollection<T>>(entry.ArchetypeCache.Value);
            return collection[entry.EntityIndex]!;
        }

        static void UpdateFilter(EntityFilter filter) => filter.Requires<T>();
    }

    static ParamGroup IQueryParam<ParamGroup>.CreateFrom(QueryParamEntityInfo entry) => default;

    static void IQueryParam<ParamGroup>.UpdateFilter(EntityFilter filter) { }
}
