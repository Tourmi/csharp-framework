namespace Tourmi.EntityComponentSystem.Archetypes;

internal partial class Archetype
{
    private struct NoData;

    /// <summary>
    /// Shared data between the archetypes of the graph.
    /// </summary>
    private class SharedData
    {
        private readonly Dictionary<Type, Func<ComponentCollection>> _componentCollectionFactories = [];

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
    }
}
