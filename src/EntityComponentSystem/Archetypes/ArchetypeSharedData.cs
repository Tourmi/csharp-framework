using Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

namespace Tourmi.EntityComponentSystem.Archetypes;

/// <summary>
/// Shared data between the archetypes of the graph.
/// </summary>
internal class ArchetypeSharedData(IdentifiersToArchetypeDictionary identifierDictionary)
{
    /// <summary>
    /// Struct representing a component with no data.
    /// </summary>
    /// <remarks>
    /// C# does not actually support zero-width structs, and this will always take up one byte
    /// of memory per entry. Need to find an alternate solution.
    /// </remarks>
    private struct NoData;

    private readonly IdentifiersToArchetypeDictionary _archetypes = identifierDictionary;
    private readonly Dictionary<Type, Func<IComponentCollection>> _componentCollectionFactories = [];

    public IComponentCollection CreateComponentCollection(Type? dataType)
    {
        if (dataType is null)
        {
            return new ComponentCollection<NoData>();
        }

        if (!_componentCollectionFactories.TryGetValue(dataType, out var factory))
        {
            var collectionType = typeof(ComponentCollection<>).MakeGenericType(dataType);
            factory = () => (IComponentCollection)Activator.CreateInstance(collectionType)!;
            _componentCollectionFactories[dataType] = factory;
        }

        return factory();
    }

    public Archetype GetArchetype(IEnumerable<KeyValuePair<Identifier, Type?>> components) => _archetypes[components];
}
