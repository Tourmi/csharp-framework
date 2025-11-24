namespace Tourmi.EntityComponentSystem.Archetypes;

internal partial class Archetype
{
    private struct NoData;

    /// <summary>
    /// Shared data between the archetypes of the graph.
    /// </summary>
    private class SharedData
    {
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

        private readonly Dictionary<Type, Func<ComponentCollection>> _componentCollectionFactories = [];
        private readonly SortedDictionary<Identifier[], Archetype> _archetypes = new(IdentifierCollectionComparer);

        public SharedData(Archetype emptyArchetype)
        {
            _archetypes[[]] = emptyArchetype;
        }

        public ComponentCollection CreateComponentCollection(Type? dataType)
        {
            if (dataType is null)
            {
                return new ComponentCollection<NoData>();
            }

            if (!_componentCollectionFactories.TryGetValue(dataType, out var factory))
            {
                var collectionType = typeof(ComponentCollection<>).MakeGenericType(dataType);
                factory = () => (ComponentCollection)Activator.CreateInstance(collectionType)!;
                _componentCollectionFactories[dataType] = factory;
            }

            return factory();
        }

        public Archetype GetArchetype(IEnumerable<KeyValuePair<Identifier, Type?>> components)
        {
            var key = components.Select(x => x.Key).Order().ToArray();
            if (!_archetypes.TryGetValue(key, out var archetype))
            {
                archetype = new Archetype(this, components);
                _archetypes[key] = archetype;
            }

            return archetype;
        }
    }
}
