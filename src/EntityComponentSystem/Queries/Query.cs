using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    private bool _isEntityCacheDirty; // TODO: Set to true by default once entity cache invalidation is done.

    internal Query(World world, params ReadOnlySpan<Identifier> componentIds)
    {
        _world = world.ThrowIfNull();
        _componentIds = [.. componentIds];

        _world.Archetypes.ComponentsToArchetypes.ArchetypeAdded += OnArchetypeAdded;
        _world.Archetypes.ComponentsToArchetypes.ArchetypeRemoved += OnArchetypeRemoved;
    }

    /// <summary>
    /// Executes the given <paramref name="action"/> for all entities returned by the query.
    /// </summary>
    public void ForEach<T>(Action<T> action)
        where T : IQueryParam<T>, allows ref struct
    {
        EnsureCache();

        var globalCache = T.GetGlobalCache(_world);

        foreach (var archetype in _cachedArchetypes)
        {
            var archetypeCache = T.GetArchetypeCache(_world, archetype, globalCache);
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                action(T.CreateFrom(new( _world, archetype, i, globalCache, archetypeCache)));
            }

            T.FreeArchetypeCache(archetypeCache, globalCache, _world, archetype);
        }

        T.FreeGlobalCache(globalCache, _world);
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
        // TODO: Set to true once entity cache invalidation is done.
        _isEntityCacheDirty = false;
        _cachedEntityEntries.Clear();
        _cachedEntityIds.Clear();
    }

    /// <summary>
    /// Returns all entity entries with their archetype that are targeted by the query.
    /// </summary>
    internal IEnumerable<ArchetypeEntityEntry> GetEntityEntries()
    {
        EnsureCache();

        // TODO: Figure out per-entity cache invalidation before uncommenting the next line.
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
        EnsureEntityCache();

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

        void EnsureEntityCache()
        {
            if (!_isEntityCacheDirty)
            {
                return;
            }

            _cachedEntityEntries.Clear();
            _cachedEntityIds.Clear();

            _isEntityCacheDirty = false;
            foreach (var archetype in _cachedArchetypes)
            {
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    _cachedEntityEntries.Add(new(archetype, i));
                    _cachedEntityIds.Add(archetype.Entities[i]);
                }
            }
        }
    }
}
