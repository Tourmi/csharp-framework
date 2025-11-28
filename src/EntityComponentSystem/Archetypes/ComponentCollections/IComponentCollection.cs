namespace Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

internal interface IComponentCollection
{
    /// <summary>
    /// Returns the count of the collection
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Adds a new entry with a default value.
    /// </summary>
    void AddEntry();

    /// <summary>
    /// Removes the entry at the given <paramref name="index"/>, swapping it with the last item in the collection.
    /// </summary>
    void RemoveEntry(int index);

    /// <summary>
    /// Steals an entry's value from the <paramref name="originalCollection"/> at the given <paramref name="index"/>.
    /// </summary>
    /// <remarks>
    /// Only works if the <paramref name="originalCollection"/>'s type is compatible with this collection's type.
    /// </remarks>
    void TakeEntryFrom(IComponentCollection originalCollection, int index);

    /// <summary>
    /// For debugging purposes, returns a boxed value for the entity at the given <paramref name="index"/>
    /// </summary>
    internal object? GetDebugValue(int index);
}
