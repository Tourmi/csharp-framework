using System.Runtime.InteropServices;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

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

    internal World World => _world;

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
    /// Generates a new query from the given query param <typeparamref name="T"/>, 
    /// and parametrizing it with the given parameters.
    /// </summary>
    public static Query FromQueryParam<T>(World world, Identifier parameter1)
        where T : IQueryParam<T>, allows ref struct
    {
        var filter = new EntityFilter(world, parameter1);
        T.UpdateFilter(filter);
        return new Query(world, filter);
    }

    /// <summary>
    /// Generates a new query from the given query param <typeparamref name="T"/>
    /// </summary>
    public static Query FromDynamicParam<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>(World world)
        where T : allows ref struct
    {
        var callbacks = ParamCallbacks.For<T>();
        var filter = new EntityFilter(world);
        callbacks.UpdateFilter(filter);
        return new Query(world, filter);
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
