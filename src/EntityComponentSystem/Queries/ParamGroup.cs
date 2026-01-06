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
        var caches = new QueryParamGlobalCache[8];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value3Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value4Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value5Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value6Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value7Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value8Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 8, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
        Value3Callbacks.FreeGlobalCache(caches[i++], world);
        Value4Callbacks.FreeGlobalCache(caches[i++], world);
        Value5Callbacks.FreeGlobalCache(caches[i++], world);
        Value6Callbacks.FreeGlobalCache(caches[i++], world);
        Value7Callbacks.FreeGlobalCache(caches[i++], world);
        Value8Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 8, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[8];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value6Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value7Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value8Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 8, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 8, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value3Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value4Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value5Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value6Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value7Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value8Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8> IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 8, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 8, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val6 = Value6Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val7 = Value7Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val8 = Value8Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2, val3, val4, val5, val6, val7, val8);
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
        var caches = new QueryParamGlobalCache[7];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value3Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value4Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value5Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value6Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value7Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 7, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
        Value3Callbacks.FreeGlobalCache(caches[i++], world);
        Value4Callbacks.FreeGlobalCache(caches[i++], world);
        Value5Callbacks.FreeGlobalCache(caches[i++], world);
        Value6Callbacks.FreeGlobalCache(caches[i++], world);
        Value7Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 7, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[7];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value6Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value7Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 7, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 7, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value3Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value4Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value5Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value6Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value7Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2, T3, T4, T5, T6, T7> IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 7, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 7, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val6 = Value6Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val7 = Value7Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2, val3, val4, val5, val6, val7);
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
        var caches = new QueryParamGlobalCache[6];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value3Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value4Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value5Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value6Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 6, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
        Value3Callbacks.FreeGlobalCache(caches[i++], world);
        Value4Callbacks.FreeGlobalCache(caches[i++], world);
        Value5Callbacks.FreeGlobalCache(caches[i++], world);
        Value6Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 6, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[6];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value6Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 6, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 6, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value3Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value4Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value5Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value6Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2, T3, T4, T5, T6> IQueryParam<ParamGroup<T1, T2, T3, T4, T5, T6>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 6, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 6, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val6 = Value6Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2, val3, val4, val5, val6);
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
        var caches = new QueryParamGlobalCache[5];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value3Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value4Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value5Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 5, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
        Value3Callbacks.FreeGlobalCache(caches[i++], world);
        Value4Callbacks.FreeGlobalCache(caches[i++], world);
        Value5Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 5, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[5];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value5Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 5, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 5, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value3Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value4Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value5Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2, T3, T4, T5> IQueryParam<ParamGroup<T1, T2, T3, T4, T5>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 5, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 5, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val5 = Value5Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2, val3, val4, val5);
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
        var caches = new QueryParamGlobalCache[4];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value3Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value4Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 4, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
        Value3Callbacks.FreeGlobalCache(caches[i++], world);
        Value4Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3, T4>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 4, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[4];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value4Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3, T4>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 4, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 4, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value3Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value4Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2, T3, T4> IQueryParam<ParamGroup<T1, T2, T3, T4>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 4, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 4, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val4 = Value4Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2, val3, val4);
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
        var caches = new QueryParamGlobalCache[3];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value3Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 3, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
        Value3Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2, T3>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 3, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[3];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value3Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2, T3>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 3, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 3, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value3Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2, T3> IQueryParam<ParamGroup<T1, T2, T3>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 3, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 3, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val3 = Value3Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2, val3);
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
        var caches = new QueryParamGlobalCache[2];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);
        i++;
        caches[i] = Value2Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 2, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
        Value2Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1, T2>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 2, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[2];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);
        i++;
        caches[i] = Value2Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1, T2>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 2, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 2, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
        i++;
        Value2Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1, T2> IQueryParam<ParamGroup<T1, T2>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 2, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 2, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });
        i++;
        var val2 = Value2Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1, val2);
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
        var caches = new QueryParamGlobalCache[1];
        var i = 0;
        caches[i] = Value1Callbacks.GetGlobalCache(world);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1>>.FreeGlobalCache(QueryParamGlobalCache existingCache, World world)
    {
        Debug.Assert(existingCache.Value is QueryParamGlobalCache[] array && array.Length == 1, "Cache didn't have expected type.");
        var caches = (QueryParamGlobalCache[])existingCache.Value!;
        var i = 0;
        Value1Callbacks.FreeGlobalCache(caches[i++], world);
    }

    static QueryParamArchetypeCache IQueryParam<ParamGroup<T1>>.GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
    {
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] array && array.Length == 1, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var caches = new QueryParamArchetypeCache[1];
        var i = 0;
        caches[i] = Value1Callbacks.GetArchetypeCache(world, archetype, globalCaches[i]);

        return new(caches);
    }

    static void IQueryParam<ParamGroup<T1>>.FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
    {
        Debug.Assert(existingCache.Value is QueryParamArchetypeCache[] array && array.Length == 1, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])existingCache.Value!;
        Debug.Assert(globalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 1, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])globalCache.Value!;
        var i = 0;
        Value1Callbacks.FreeArchetypeCache(caches[i], globalCaches[i], world, archetype);
    }

    static ParamGroup<T1> IQueryParam<ParamGroup<T1>>.CreateFrom(QueryParamEntityInfo entry)
    {
        Debug.Assert(entry.ArchetypeCache.Value is QueryParamArchetypeCache[] array && array.Length == 1, "Cache didn't have expected type.");
        var caches = (QueryParamArchetypeCache[])entry.ArchetypeCache.Value!;
        Debug.Assert(entry.GlobalCache.Value is QueryParamGlobalCache[] globalArray && globalArray.Length == 1, "Cache didn't have expected type.");
        var globalCaches = (QueryParamGlobalCache[])entry.GlobalCache.Value!;

        var i = 0;
        var val1 = Value1Callbacks.CreateFrom(entry with { GlobalCache = globalCaches[i], ArchetypeCache = caches[i] });

        return new(val1);
    }
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup() : IParamGroup, IQueryParam<ParamGroup>
{
    internal record ParamCallbacks<T>(
        Func<World, QueryParamGlobalCache> GetGlobalCache,
        Action<QueryParamGlobalCache, World> FreeGlobalCache,
        Func<World, Archetype, QueryParamGlobalCache, QueryParamArchetypeCache> GetArchetypeCache,
        Action<QueryParamArchetypeCache, QueryParamGlobalCache, World, Archetype> FreeArchetypeCache,
        Func<QueryParamEntityInfo, T> CreateFrom
    ) where T : allows ref struct;

    /// <inheritdoc/>
    public int Count => 0;

    internal static ParamCallbacks<T> GetCallbacksFor<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>() where T : allows ref struct
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
#pragma warning disable IL2055 // Either the type on which the MakeGenericType is called can't be statically determined, or the type parameters to be used for generic arguments can't be statically determined.

        var mapping = typeof(T).GetInterfaceMap(interfaceType)!;
#pragma warning restore IL2055
#pragma warning restore IL2062

        var getGlobalCache = GetDelegate<Func<World, QueryParamGlobalCache>>(mapping, nameof(IQueryParam<>.GetGlobalCache));
        var freeGlobalCache = GetDelegate<Action<QueryParamGlobalCache, World>>(mapping, nameof(IQueryParam<>.FreeGlobalCache));
        var getArchetypeCache = GetDelegate<Func<World, Archetype, QueryParamGlobalCache, QueryParamArchetypeCache>>(mapping, nameof(IQueryParam<>.GetArchetypeCache));
        var freeArchetypeCache = GetDelegate<Action<QueryParamArchetypeCache, QueryParamGlobalCache, World, Archetype>>(mapping, nameof(IQueryParam<>.FreeArchetypeCache));
        var createFrom = GetDelegate<Func<QueryParamEntityInfo, T>>(mapping, nameof(IQueryParam<>.CreateFrom));

        return new(getGlobalCache, freeGlobalCache, getArchetypeCache, freeArchetypeCache, createFrom);

        static TDelegate GetDelegate<TDelegate>(InterfaceMapping mapping, string methodName) where TDelegate : Delegate
        {
            var targetMethod = mapping.TargetMethods[mapping
                    .InterfaceMethods
                    .Index()
                    .First(m => m.Item.Name == methodName)
                    .Index];
            
            if (targetMethod.IsVirtual)
            {
                var interfaceType = typeof(DummyQueryParam).GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryParam<>)).First();
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
                var newMapping = typeof(DummyQueryParam).GetInterfaceMap(interfaceType);
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.

                return GetDelegate<TDelegate>(newMapping, methodName);
            }

            return targetMethod.CreateDelegate<TDelegate>();
        }
    }

    private static ParamCallbacks<T> GetDefaultCallbacksFor<T>()
    {
        return new(GetGlobalCache, FreeGlobalCache, GetArchetypeCache, FreeArchetypeCache, CreateFrom);

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
            pool.Return((StrongBox<Identifier>)existingCache.Value!);
        }


        static QueryParamArchetypeCache GetArchetypeCache(World world, Archetype archetype, QueryParamGlobalCache globalCache)
        {
            Debug.Assert(globalCache.Value is StrongBox<Identifier>, "Given cache was of the wrong type.");

            var componentId = ((StrongBox<Identifier>)globalCache.Value!).Value;

            return new(archetype.GetComponentCollection<T>(componentId));
        }

        static void FreeArchetypeCache(QueryParamArchetypeCache existingCache, QueryParamGlobalCache globalCache, World world, Archetype archetype)
        {
            // Nothing to free
        }

        static T CreateFrom(QueryParamEntityInfo entry)
        {
            Debug.Assert(entry.ArchetypeCache.Value is IComponentCollection<T>, "Given cache was of the wrong type.");
            var collection = (IComponentCollection<T>)entry.ArchetypeCache.Value!;
            return collection[entry.EntityIndex]!;
        }
    }

    static ParamGroup IQueryParam<ParamGroup>.CreateFrom(QueryParamEntityInfo entry) => default;
}
