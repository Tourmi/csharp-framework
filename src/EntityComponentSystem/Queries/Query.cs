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

        // TODO: Figure out per-entity caches before uncommenting the next line.
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

        // TODO: Figure out per-entity caches before uncommenting the next line.
        // return _cachedEntityIds;

        foreach (var archetype in _cachedArchetypes)
        {
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                yield return archetype.Entities[i];
            }
        }
    }

    internal static Type GetQueryTypeFromDelegate<T>(T del) where T : Delegate
    {
        // TODO: instead of returning a type, it should probably be a list of identifiers, similar to Archetypes
        // TODO: use some form of cache to avoid GCing the list on each call
        var paramGroup = new List<QueryParamInfo>();
        var parameters = del.Method.GetParameters();
        foreach (var param in parameters)
        {
            ParseTypeInto(paramGroup, param.ToType());
        }

        if (del.Method.ReturnType != typeof(void))
        {
            if (del.Method.ReturnType.IsByRef || del.Method.ReturnType.IsByRefLike)
            {
                throw new NotSupportedException("Cannot create a query from a delegate that returns a ref, or refstruct value.");
            }

            ParseTypeInto(paramGroup, typeof(OutRef<>).MakeGenericType(del.Method.ReturnType));
        }

        if (paramGroup.Count == 0)
        {
            return typeof(Query);
        }

        if (paramGroup.Count == 1)
        {
            return typeof(Query<>).MakeGenericType(paramGroup[0].ParamType);
        }

        var recursiveDepth = (paramGroup.Count - 2) / 7 + 1;
        var lastDepthCount = paramGroup.Count - (recursiveDepth - 1) * 7;

        var previousQueryParamType = lastDepthCount switch
        {
            2 => typeof(ParamGroup<,>).MakeGenericType(paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            3 => typeof(ParamGroup<,,>).MakeGenericType(paramGroup[^3].ParamType, paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            4 => typeof(ParamGroup<,,,>).MakeGenericType(paramGroup[^4].ParamType, paramGroup[^3].ParamType, paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            5 => typeof(ParamGroup<,,,,>).MakeGenericType(paramGroup[^5].ParamType, paramGroup[^4].ParamType, paramGroup[^3].ParamType, paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            6 => typeof(ParamGroup<,,,,,>).MakeGenericType(paramGroup[^6].ParamType, paramGroup[^5].ParamType, paramGroup[^4].ParamType, paramGroup[^3].ParamType, paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            7 => typeof(ParamGroup<,,,,,,>).MakeGenericType(paramGroup[^7].ParamType, paramGroup[^6].ParamType, paramGroup[^5].ParamType, paramGroup[^4].ParamType, paramGroup[^3].ParamType, paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            8 => typeof(ParamGroup<,,,,,,,>).MakeGenericType(paramGroup[^8].ParamType, paramGroup[^7].ParamType, paramGroup[^6].ParamType, paramGroup[^5].ParamType, paramGroup[^4].ParamType, paramGroup[^3].ParamType, paramGroup[^2].ParamType, paramGroup[^1].ParamType),
            _ => null!,
        };

        recursiveDepth--;
        while (recursiveDepth > 0)
        {
            var offset = recursiveDepth * 7;
            previousQueryParamType = typeof(ParamGroup<,,,,,,,>).MakeGenericType(
                paramGroup[offset + 0].ParamType,
                paramGroup[offset + 1].ParamType,
                paramGroup[offset + 2].ParamType,
                paramGroup[offset + 3].ParamType,
                paramGroup[offset + 4].ParamType,
                paramGroup[offset + 5].ParamType,
                paramGroup[offset + 6].ParamType,
                previousQueryParamType);

            recursiveDepth--;
        }

        return typeof(Query<>).MakeGenericType(previousQueryParamType);

        static void ParseTypeInto(List<QueryParamInfo> paramGroup, Type param)
        {
            paramGroup.Add(new(param));
        }
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

        // TODO: Invalidate per-entity caches (or figure out if they're needed) 
        // before uncommenting next lines.

        /* 
        _isEntityCacheDirty = false;
        foreach (var archetype in _cachedArchetypes)
        {
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                _cachedEntityEntries.Add(new(archetype, i));
                _cachedEntityIds.Add(archetype.Entities[i]);
            }
        }
        */

        return;

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
