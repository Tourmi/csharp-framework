using System.Runtime.InteropServices;

namespace Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

/// <inheritdoc/>
internal class ComponentCollection<T> : IComponentCollection<T>
{
    private readonly List<T?> _values = [];

    /// <inheritdoc/>
    public int Count => _values.Count;

    /// <inheritdoc/>
    public ref T? this[int index] => ref AsSpan()[index];

    /// <inheritdoc/>
    public void AddEntry() => _values.Add(default);

    /// <inheritdoc/>
    public void TakeEntryFrom(IComponentCollection originalCollection, int index)
    {
        if (originalCollection is not ICovariantComponentCollection<T> collection)
        {
            throw new ArgumentOutOfRangeException(nameof(originalCollection));
        }

        _values.Add(collection[index]);
        collection.RemoveEntry(index);
    }

    /// <inheritdoc/>
    public void RemoveEntry(int index) => _values.RemoveSwap(index);

    /// <inheritdoc/>
    public Span<T?> AsSpan() => CollectionsMarshal.AsSpan(_values);

    /// <inheritdoc/>
    public ComponentCollectionEnumerator<T> GetEnumerator() => new(this);

    /// <inheritdoc/>
    T? ICovariantComponentCollection<T>.this[int index] => this[index];

    /// <inheritdoc/>
    T? IContravariantComponentCollection<T>.this[int index]
    {
        set => this[index] = value;
    }

    /// <inheritdoc/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    /// <inheritdoc/>
    object? IComponentCollection.GetDebugValue(int index) => this[index];
}
