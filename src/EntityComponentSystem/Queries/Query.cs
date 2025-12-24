using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Queries the <see cref="World"/> for entities filtered by the components of the <see cref="Query"/>.
/// </summary>
public sealed class Query : IDisposable
{
    private readonly World _world;
    private readonly Identifier[] _componentIds;
    private readonly HashSet<Archetype> _cachedArchetypes = [];
    private readonly List<ArchetypeEntityEntry> _cachedEntityEntries = [];
    private readonly List<Identifier> _cachedEntityIds = [];

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
    /// Returns all entity entries with their archetype that are targeted by the query.
    /// </summary>
    internal IEnumerable<ArchetypeEntityEntry> GetEntityEntries()
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
    /// Returns all entity Ids for the query. 
    /// Useful when general operations on the entity are needed.
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

    private void OnArchetypeAdded(Archetype archetype)
    {
        for (var i = 0; i < _componentIds.Length; i++)
        {
            if (!archetype.Components.Contains(_componentIds[i]))
            {
                return;
            }
        }

        _cachedArchetypes.Add(archetype);
    }

    private void OnArchetypeRemoved(Archetype archetype) => _cachedArchetypes.Remove(archetype);

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

            if (_componentIds.Length == 0)
            {
                _cachedArchetypes.UnionWith(_world.Archetypes.GetArchetypes());
                return;
            }

            _cachedArchetypes.UnionWith(_world.Archetypes.GetArchetypesContainingComponent(_componentIds[0]));
            for (var i = 1; i < _componentIds.Length; i++)
            {
                if (_cachedArchetypes.Count == 0)
                {
                    return;
                }

                _cachedArchetypes.IntersectWith(_world.Archetypes.GetArchetypesContainingComponent(_componentIds[i]));
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
