using System;
using System.Reflection;
using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Queries the <see cref="World"/> for entities filtered by the type parameters of the <see cref="Query"/>.
/// </summary>
public sealed class Query : IDisposable
{
    private readonly World _world;
    private readonly Identifier[] _componentIds;
    private readonly HashSet<Archetype> _cachedArchetypes = [];
    private readonly HashSet<ArchetypeEntityEntry> _cachedEntityEntries = [];
    private readonly HashSet<Identifier> _cachedEntityIds = [];

    private bool _isArchetypeCacheDirty = true;
    private bool _isEntityCacheDirty = true;

    internal Query(World world, params IEnumerable<Identifier> componentIds)
    {
        _world = world.ThrowIfNull();
        _componentIds = [.. componentIds.ThrowIfNull()];

        _world.Archetypes.ComponentsToArchetypes.ArchetypeAdded += OnArchetypeAdded;
        _world.Archetypes.ComponentsToArchetypes.ArchetypeRemoved += OnArchetypeRemoved;
    }

    /// <summary>
    /// Disposes the query, freeing resources.
    /// </summary>
    public void Dispose()
    {
        _cachedArchetypes.Clear();
        _cachedEntityEntries.Clear();
        _cachedEntityIds.Clear();
        _world.Archetypes.ComponentsToArchetypes.ArchetypeAdded -= OnArchetypeAdded;
        _world.Archetypes.ComponentsToArchetypes.ArchetypeRemoved -= OnArchetypeRemoved;
    }

    /// <summary>
    /// Invalidates the cached archetypes in this query.
    /// </summary>
    internal void InvalidateArchetypeCache()
    {
        InvalidateEntityCache();
        _isArchetypeCacheDirty = true;
        _cachedArchetypes.Clear();
    }

    /// <summary>
    /// Invalidates the cached entitities in this query.
    /// </summary>
    internal void InvalidateEntityCache()
    {
        _isEntityCacheDirty = true;
        _cachedEntityEntries.Clear();
        _cachedEntityIds.Clear();
    }

    /// <summary>
    /// Returns all entity entries for the archetype.
    /// </summary>
    internal IEnumerable<ArchetypeEntityEntry> GetEntities()
    {
        EnsureCache();

        // TODO: Invalidate per-entity caches before uncommenting the next line.
        // return _cachedEntityEntries;

        foreach (var archetype in _cachedArchetypes)
        {
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                yield return new(archetype, i);
            }
        }
    }

    /// <summary>
    /// Returns all entity Ids for the archetype. 
    /// Useful when generic operations on the entity are needed.
    /// </summary>
    internal IEnumerable<Identifier> GetEntityIds()
    {
        EnsureCache();

        // TODO: Invalidate per-entity caches before uncommenting the next line.
        // return _cachedEntityIds;

        foreach (var archetype in _cachedArchetypes)
        {
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                yield return archetype.Entities[i];
            }
        }
    }


    internal static Type GetQueryTypeFromDelegate(Delegate del)
    {
        // TODO: instead of returning a type, it should probably be a list of identifiers, similar to Archetypes
        // TODO: use some form of cache to avoid GCing the list on each call
        var queryParams = new List<QueryParamInfo>();
        var parameters = del.Method.GetParameters();
        foreach (var param in parameters)
        {
            ParseTypeInto(queryParams, ParamToType(param));
        }

        if (del.Method.ReturnType != typeof(void))
        {
            if (del.Method.ReturnType.IsByRef || del.Method.ReturnType.IsByRefLike)
            {
                throw new NotSupportedException("Cannot create a query from a delegate that returns a ref, or refstruct value.");
            }

            ParseTypeInto(queryParams, typeof(OutRef<>).MakeGenericType(del.Method.ReturnType));
        }

        if (queryParams.Count == 0)
        {
            return typeof(Query);
        }

        if (queryParams.Count == 1)
        {
            return typeof(Query<>).MakeGenericType(queryParams[0].ParamType);
        }

        var recursiveDepth = (queryParams.Count - 2) / 7 + 1;
        var lastDepthCount = queryParams.Count - (recursiveDepth - 1) * 7;

        var previousQueryParamType = lastDepthCount switch
        {
            2 => typeof(QueryParams<,>).MakeGenericType(queryParams[^2].ParamType, queryParams[^1].ParamType),
            3 => typeof(QueryParams<,,>).MakeGenericType(queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            4 => typeof(QueryParams<,,,>).MakeGenericType(queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            5 => typeof(QueryParams<,,,,>).MakeGenericType(queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            6 => typeof(QueryParams<,,,,,>).MakeGenericType(queryParams[^6].ParamType, queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            7 => typeof(QueryParams<,,,,,,>).MakeGenericType(queryParams[^7].ParamType, queryParams[^6].ParamType, queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            8 => typeof(QueryParams<,,,,,,,>).MakeGenericType(queryParams[^8].ParamType, queryParams[^7].ParamType, queryParams[^6].ParamType, queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            _ => null!,
        };

        recursiveDepth--;
        while (recursiveDepth > 0)
        {
            var offset = recursiveDepth * 7;
            previousQueryParamType = typeof(QueryParams<,,,,,,,>).MakeGenericType(
                queryParams[offset + 0].ParamType,
                queryParams[offset + 1].ParamType,
                queryParams[offset + 2].ParamType,
                queryParams[offset + 3].ParamType,
                queryParams[offset + 4].ParamType,
                queryParams[offset + 5].ParamType,
                queryParams[offset + 6].ParamType,
                previousQueryParamType);

            recursiveDepth--;
        }

        return typeof(Query<>).MakeGenericType(previousQueryParamType);
    }

    internal void AddSystem(Delegate system)
    {

    }

    private static Type ParamToType(ParameterInfo param)
    {
        var elementType = param.ParameterType;
        if (elementType.IsByRef)
        {
            elementType = elementType.GetElementType()!;
        }

        if (!elementType.IsValueType)
        {
            if ((!param.ParameterType.IsByRef || param.IsIn) && param.GetCustomAttribute<ThreadSafeAttribute>() is not null)
            {
                return typeof(ThreadSafe<>).MakeGenericType(elementType);
            }

            return elementType;
        }

        if (elementType.IsByRefLike)
        {
            if (param.ParameterType.IsByRef)
            {
                throw new NotSupportedException("Cannot build query with a ref struct parameter passed via ref/out/in");
            }

            return elementType;
        }

        if (!param.ParameterType.IsByRef)
        {
            return elementType;
        }

        if (param.IsIn)
        {
            return typeof(RefReadonly<>).MakeGenericType(elementType);
        }

        if (param.IsOut)
        {
            return typeof(OutRef<>).MakeGenericType(elementType);
        }

        return typeof(Ref<>).MakeGenericType(elementType);
    }

    private static void ParseTypeInto(List<QueryParamInfo> queryParams, Type param)
    {
        queryParams.Add(new(param));
    }

    private void OnArchetypeAdded(Archetype archetype)
    {
        for (var i = 0; i < _componentIds.Length; i++)
        {
            if (archetype.Components.Contains(_componentIds[i]))
            {
                _cachedArchetypes.Add(archetype);
                return;
            }
        }
    }

    private void OnArchetypeRemoved(Archetype archetype)
    {
        for (var i = 0; i < _componentIds.Length; i++)
        {
            if (archetype.Components.Contains(_componentIds[i]))
            {
                _cachedArchetypes.Remove(archetype);
                return;
            }
        }
    }

    private void EnsureCache()
    {
        EnsureArchetypeCache();

        if (!_isEntityCacheDirty)
        {
            return;
        }

        _cachedEntityEntries.Clear();
        _cachedEntityIds.Clear();

        // TODO: Invalidate per-entity caches before uncommenting the next line.
        // _isEntityCacheDirty = false;

        return;

        foreach (var archetype in _cachedArchetypes)
        {
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                _cachedEntityEntries.Add(new(archetype, i));
                _cachedEntityIds.Add(archetype.Entities[i]);
            }
        }

        void EnsureArchetypeCache()
        {
            if (!_isArchetypeCacheDirty)
            {
                return;
            }

            _cachedArchetypes.Clear();
            _isArchetypeCacheDirty = false;

            // TODO: Support empty query returning all entities
            if (_componentIds.Length == 0)
            {
                throw new NotImplementedException("Queries with no component ids are not supported yet.");
            }

            _cachedArchetypes.UnionWith(_world.Archetypes.GetArchetypesContainingComponent(_componentIds[0]));
            for (var i = 1; i < _componentIds.Length; i++)
            {
                if (_cachedArchetypes.Count == 0)
                {
                    return;
                }

                _cachedArchetypes.IntersectWith(_world.Archetypes.GetArchetypesContainingComponent(_componentIds[0]));
            }
        }
    }

    private readonly record struct QueryParamInfo(Type ParamType);
}

/// <inheritdoc cref="Query"/>
public sealed class Query<T1>
    where T1 : allows ref struct;

/// <inheritdoc cref="Query"/>
public sealed class Query<T1, T2>
    where T1 : allows ref struct
    where T2 : allows ref struct;

/// <inheritdoc cref="Query"/>
public sealed class Query<T1, T2, T3>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct;

/// <inheritdoc cref="Query"/>
public sealed class Query<T1, T2, T3, T4>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3, T4, T5>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct;

/// <inheritdoc cref="Query"/>
public sealed class Query<T1, T2, T3, T4, T5, T6>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct;

/// <inheritdoc cref="Query"/>
public sealed class Query<T1, T2, T3, T4, T5, T6, T7>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct;

/// <inheritdoc cref="Query"/>
public sealed class Query<T1, T2, T3, T4, T5, T6, T7, T8>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
    where T8 : allows ref struct;
