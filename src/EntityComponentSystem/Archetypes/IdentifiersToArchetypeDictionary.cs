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
    public event Action<Archetype>? ArchetypeRemoved;

    private static readonly IComparer<Identifier[]> IdentifierCollectionComparer = Comparer.FromFunc((Identifier[] c1, Identifier[] c2) =>
    {
        var compare = c1.Length.CompareTo(c2.Length);
        if (compare != 0)
        {
            return compare;
        }

        for (var i = 0; i < c1.Length; i++)
        {
            compare = c1[i].CompareTo(c2[i]);
            if (compare != 0)
            {
                return compare;
            }
        }

        return 0;
    });

    private readonly SortedDictionary<Identifier[], Archetype> _archetypes = new(IdentifierCollectionComparer);
    private readonly Dictionary<Identifier, List<Archetype>> _archetypeGroups = [];
    private readonly ArchetypeSharedData _sharedData;
    private readonly Archetype _emptyArchetype;

    public IdentifiersToArchetypeDictionary()
    {
        _sharedData = new(this);
        _emptyArchetype = new(_sharedData);
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
}