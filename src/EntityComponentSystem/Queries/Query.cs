using System.Runtime.InteropServices;
using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Queries the <see cref="World"/> for entities filtered by the components of the <see cref="Query"/>.
/// </summary>
public sealed class Query : IDisposable
{
    private readonly World _world;
    private readonly EntityFilter _filter;
    private readonly List<Archetype> _cachedArchetypes = [];

    private bool _isArchetypeCacheDirty = true;

    internal Query(World world, EntityFilter filter)
    {
        _world = world.ThrowIfNull();
        _filter = filter;

        _world.Archetypes.ComponentsToArchetypes.ArchetypeAdded += OnArchetypeAdded;
        _world.Archetypes.ComponentsToArchetypes.ArchetypeRemoved += OnArchetypeRemoved;
    }

    /// <summary>
    /// Generates a new query from the given query param <typeparamref name="T"/>
    /// </summary>
    public static Query FromQueryParam<T>(World world)
        where T : IQueryParam<T>, allows ref struct
    {
        var filter = new EntityFilter(world);
        T.UpdateFilter(filter);
        return new Query(world, filter);
    }

    /// <summary>
    /// Executes the given <paramref name="action"/> for all entities returned by the query.
    /// </summary>
    public void ForEach<T>(Action<T> action)
        where T : IQueryParam<T>, allows ref struct
    {
        var globalCache = T.GetGlobalCache(_world);

        foreach (var archetype in GetArchetypes())
        {
            var archetypeCache = T.GetArchetypeCache(_world, archetype, globalCache);
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                action(T.CreateFrom(new(_world, archetype, i, globalCache, archetypeCache)));
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
        _world.Archetypes.ComponentsToArchetypes.ArchetypeAdded -= OnArchetypeAdded;
        _world.Archetypes.ComponentsToArchetypes.ArchetypeRemoved -= OnArchetypeRemoved;
    }

    /// <summary>
    /// Returns the query's matching archetypes
    /// </summary>
    internal ReadOnlySpan<Archetype> GetArchetypes() => CollectionsMarshal.AsSpan(GetArchetypesInternal());

    /// <summary>
    /// Returns all entity entries with their archetype that are targeted by the query.
    /// </summary>
    internal IEnumerable<ArchetypeEntityEntry> GetEntityEntries()
    {
        foreach (var archetype in GetArchetypesInternal())
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
        foreach (var archetype in GetArchetypesInternal())
        {
            for (var i = 0; i < archetype.EntityCount; i++)
            {
                yield return archetype.Entities[i];
            }
        }
    }

    private List<Archetype> GetArchetypesInternal()
    {
        if (!_isArchetypeCacheDirty)
        {
            return _cachedArchetypes;
        }

        _cachedArchetypes.Clear();
        _isArchetypeCacheDirty = false;

        foreach (var archetype in _world.Archetypes.GetArchetypes())
        {
            if (_filter.ArchetypeMatches(archetype))
            {
                _cachedArchetypes.Add(archetype);
            }
        }

        return _cachedArchetypes;
    }

    private void OnArchetypeAdded(Archetype archetype)
    {
        if (_filter.ArchetypeMatches(archetype))
        {
            _cachedArchetypes.Add(archetype);
        }
    }

    private void OnArchetypeRemoved(Archetype archetype) => _cachedArchetypes.Remove(archetype);
}
