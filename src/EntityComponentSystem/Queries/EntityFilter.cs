namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Contains information that queries use to filter out entities.
/// </summary>
public sealed class EntityFilter
{
    private readonly World _world;
    private readonly HashSet<Identifier> _requiredComponentIds = [];
    private readonly HashSet<Identifier> _excludedComponentIds = [];

    internal EntityFilter(World world)
    {
        _world = world;
    }

    internal EntityFilter(World world, IEnumerable<Identifier> requiredIds) : this(world)
    {
        foreach (var id in requiredIds)
        {
            _requiredComponentIds.Add(id);
        }
    }

    /// <summary>
    /// Specifies that a component of type <typeparamref name="T"/> is required.
    /// </summary>
    public void Requires<T>() => Requires(_world.GetComponentForType<T>());

    /// <summary>
    /// Specifies that a component with the given <paramref name="componentId"/> is required.
    /// </summary>
    public void Requires(Identifier componentId) => _requiredComponentIds.Add(componentId);

    /// <summary>
    /// Specifies that entities with the component <typeparamref name="T"/> should be excluded.
    /// </summary>
    public void Excludes<T>() => Excludes(_world.GetComponentForType<T>());

    /// <summary>
    /// Specifies that entities with the component represented by the given <paramref name="componentId"/> should be excluded.
    /// </summary>
    public void Excludes(Identifier componentId) => _excludedComponentIds.Add(componentId);

    /// <summary>
    /// Returns <see langword="true"/> if the given <paramref name="archetype"/> passes this filter.
    /// </summary>
    internal bool ArchetypeMatches(Archetype archetype)
    {
        _ = archetype.ThrowIfNull();

        foreach (var componentId in archetype.Components)
        {
            if (_excludedComponentIds.Contains(componentId))
            {
                return false;
            }
        }

        foreach (var requiredComponentId in _requiredComponentIds)
        {
            if (!archetype.Components.Contains(requiredComponentId))
            {
                return false;
            }
        }

        return true;
    }
}
