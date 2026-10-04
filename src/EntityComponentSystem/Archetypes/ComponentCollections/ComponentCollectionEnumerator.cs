
namespace Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

internal ref struct ComponentCollectionEnumerator<T>(IComponentCollection<T> collection) : IEnumerator<T?>
{
    private readonly Span<T?> _collectionSpan = collection.AsSpan();
    private int _currentIndex = -1;

    public readonly ref T? Current
    {
        get
        {
            if (_currentIndex < 0)
            {
                throw new InvalidOperationException("");
            }

            return ref _collectionSpan[_currentIndex];
        }
    }

    readonly T? IEnumerator<T?>.Current => Current;

    readonly object? IEnumerator.Current => Current;

    /// <inheritdoc/>
    public bool MoveNext()
    {
        if (_currentIndex >= _collectionSpan.Length - 1)
        {
            return false;
        }

        _currentIndex++;
        return true;
    }

    readonly void IEnumerator.Reset() => throw new NotSupportedException();

    readonly void IDisposable.Dispose() { }
}
