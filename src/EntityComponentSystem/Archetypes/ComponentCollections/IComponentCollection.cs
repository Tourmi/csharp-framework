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
    /// Steals an entry's value from the <paramref name="sourceCollection"/> at the given <paramref name="index"/>.
    /// </summary>
    /// <remarks>
    /// Only works if the <paramref name="sourceCollection"/>'s type is compatible with this collection's type.
    /// </remarks>
    void TakeEntryFrom(IComponentCollection sourceCollection, int index);

    /// <summary>
    /// Overwrites the value at <paramref name="targetIndex"/> in this collection 
    /// based on the value in the <paramref name="sourceCollection"/>'s <paramref name="sourceIndex"/>.
    /// </summary>
    void CopyValueFrom(IComponentCollection sourceCollection, int sourceIndex, int targetIndex);

    /// <summary>
    /// Clears all entries for the collection.
    /// </summary>
    void Clear();

    /// <summary>
    /// For debugging purposes, returns a boxed value for the entity at the given <paramref name="index"/>
    /// </summary>
    internal object? GetDebugValue(int index);
}
