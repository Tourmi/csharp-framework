namespace Tourmi.EntityComponentSystem.Archetypes;

internal class IdentifiersToArchetypeDictionary
{
    /// <summary>
    /// Raised when a new archetype is created.
    /// </summary>
    public event Action<Archetype>? ArchetypeAdded;

    /// <summary>
    /// Raised when an archetype is deleted. (Usually when reclaiming free space)
    /// </summary>
#pragma warning disable CS0067 // Event will be used in the future
    public event Action<Archetype>? ArchetypeRemoved;
#pragma warning restore CS0067

    private readonly SortedDictionary<Identifier[], Archetype> _archetypes = new(Identifier.IdentifierCollectionComparer);
    private readonly Dictionary<Identifier, List<Archetype>> _archetypeGroups = [];
    private readonly ArchetypeSharedData _sharedData;
    private readonly Archetype _emptyArchetype;

    public IdentifiersToArchetypeDictionary()
    {
        _sharedData = new(this);
        _emptyArchetype = new(_sharedData);
        _archetypes[[]] = _emptyArchetype;
    }

    /// <summary>
    /// Archetype for entities containing no components.
    /// </summary>
    public Archetype EmptyArchetype => _emptyArchetype;

    /// <summary>
    /// Returns all archetypes containing the given <paramref name="componentId"/>
    /// </summary>
    public IEnumerable<Archetype> this[Identifier componentId]
        => _archetypeGroups.TryGetValue(componentId, out var archetypes) ? archetypes : [];

    /// <summary>
    /// Returns the archetype represented by the given <paramref name="components"/>, 
    /// creating it if needed.
    /// </summary>
    public Archetype this[IEnumerable<KeyValuePair<Identifier, Type?>> components]
    {
        get
        {
            var key = components.Select(x => x.Key).Order().ToArray();
            if (!_archetypes.TryGetValue(key, out var archetype))
            {
                archetype = new Archetype(_sharedData, components);
                _archetypes[key] = archetype;
                foreach (var component in archetype.Components)
                {
                    if (!_archetypeGroups.TryGetValue(component, out var group))
                    {
                        group = [];
                        _archetypeGroups[component] = group;
                    }

                    group.Add(archetype);
                }

                ArchetypeAdded?.Invoke(archetype);
            }

            return archetype;
        }
    }

    /// <summary>
    /// Returns all the archetypes contained by the dictionary.
    /// </summary>
    public IReadOnlyCollection<Archetype> Values => _archetypes.Values;
}