namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Contains information that queries use to filter out entities.
/// </summary>
public sealed class EntityFilter
{
    private readonly World _world;
    private readonly Identifier[] _parameters = new Identifier[8];

    private readonly HashSet<Identifier> _requiredComponentIds = [];
    private readonly HashSet<Identifier> _excludedComponentIds = [];

    internal EntityFilter(World world)
    {
        _world = world;
    }

    internal EntityFilter(World world, Identifier parameter1) : this(world)
    {
        _parameters[0] = parameter1;
    }

    /// <summary>
    /// Specifies that a component of type <typeparamref name="T"/> is required.
    /// </summary>
    public void Requires<T>() => _requiredComponentIds.Add(GetParametrizedComponentId<T>());

    /// <summary>
    /// Specifies that a component with the given <paramref name="componentId"/> is required.
    /// </summary>
    public void Requires(Identifier componentId) => _requiredComponentIds.Add(GetParametrizedComponentId(componentId));

    /// <summary>
    /// Specifies that entities with the component <typeparamref name="T"/> should be excluded.
    /// </summary>
    public void Excludes<T>() => _excludedComponentIds.Add(GetParametrizedComponentId<T>());

    /// <summary>
    /// Specifies that entities with the component represented by the given <paramref name="componentId"/> should be excluded.
    /// </summary>
    public void Excludes(Identifier componentId) => _excludedComponentIds.Add(GetParametrizedComponentId(componentId));

    /// <summary>
    /// Returns <see langword="true"/> if the given <paramref name="archetype"/> passes this filter.
    /// </summary>
    internal bool ArchetypeMatches(Archetype archetype)
    {
        _ = archetype.ThrowIfNull();

        foreach (var componentId in archetype.Components)
        {
            foreach (var excludedComponent in _excludedComponentIds)
            {
                if (componentId == excludedComponent)
                {
                    return false;
                }

                if (!componentId.IsRelationId || !excludedComponent.IsRelationId)
                {
                    continue;
                }

                var relationId = componentId.ToRelationId();
                var excludedRelationId = excludedComponent.ToRelationId();

                if (excludedRelationId.Target == FixedIds.Relations.Wildcard.ShortId)
                {
                    if (excludedRelationId.BuiltInRelationTypeOrNull is BuiltInRelationType.Wildcard)
                    {
                        return false;
                    }

                    if (excludedRelationId.RelationType == relationId.RelationType)
                    {
                        return false;
                    }

                    continue;
                }

                if (excludedRelationId.BuiltInRelationTypeOrNull is BuiltInRelationType.Wildcard)
                {
                    if (relationId.Target == excludedRelationId.Target)
                    {
                        return false;
                    }
                }
            }
        }

        foreach (var requiredComponentId in _requiredComponentIds)
        {
            var hasComponent = false;

            foreach (var componentId in archetype.Components)
            {
                if (componentId == requiredComponentId)
                {
                    hasComponent = true;
                    break;
                }

                if (!componentId.IsRelationId || !requiredComponentId.IsRelationId)
                {
                    continue;
                }

                var relationId = componentId.ToRelationId();
                var requiredRelationId = requiredComponentId.ToRelationId();

                if (requiredRelationId.Target == FixedIds.Relations.Wildcard.ShortId)
                {
                    if (requiredRelationId.BuiltInRelationTypeOrNull is BuiltInRelationType.Wildcard)
                    {
                        hasComponent = true;
                        break;
                    }

                    if (requiredRelationId.RelationType == relationId.RelationType)
                    {
                        hasComponent = true;
                        break;
                    }

                    continue;
                }

                if (requiredRelationId.BuiltInRelationTypeOrNull is BuiltInRelationType.Wildcard)
                {
                    if (relationId.Target == requiredRelationId.Target)
                    {
                        hasComponent = true;
                        break;
                    }
                }
            }

            if (!hasComponent)
            {
                return false;
            }
        }

        return true;
    }

    private Identifier GetParametrizedComponentId<T>() => GetParametrizedComponentId(_world.GetEntityForType<T>());

    private Identifier GetParametrizedComponentId(Identifier id)
    {
        if (id.IsRelationId)
        {
            var relationId = id.ToRelationId();
            if (TryParametrizeId(relationId.Target, out var newTarget))
            {
                return relationId.WithTarget(newTarget.ShortId);
            }

            return id;
        }

        if (TryParametrizeId(id, out var parametrizedId))
        {
            return parametrizedId;
        }

        return id;
    }

    private bool TryParametrizeId(Identifier id, out Identifier parametrizedId)
    {
        if (id >= FixedIds.Special.Parameter1 && id <= FixedIds.Special.Parameter8)
        {
            parametrizedId = _parameters[id.ShortId - FixedIds.Special.Parameter1.ShortId];
            return true;
        }

        parametrizedId = id;
        return false;
    }
}
