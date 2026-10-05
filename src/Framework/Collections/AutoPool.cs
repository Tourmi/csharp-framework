using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Collections;

/// <summary>
/// Pool that automatically creates new items via the given <paramref name="itemFactory"/> when needed.
/// New items must not be manually pushed back into this.
/// This class is not thread safe.
/// </summary>
public class AutoPool<T>(Func<T> itemFactory) : IPool<T>
    where T : class
{
    private readonly Func<T> _itemFactory = itemFactory.ThrowIfNull();
    private readonly Stack<T> _items = [];

    /// <inheritdoc/>
    public int Count => _items.Count;

    /// <inheritdoc/>
    public int Size { get; private set; }

    /// <summary>
    /// Returns an item, either taken from the pool, or generated from the pool's factory.
    /// </summary>
    public T Pop()
    {
        if (!TryPop(out var item))
        {
            item = _itemFactory();
            Size++;
        }

        return item;
    }

    /// <inheritdoc/>
    public bool TryPop([NotNullWhen(true)] out T? item) => _items.TryPop(out item);

    /// <inheritdoc/>
    public void Push(T item)
    {
        _items.Push(item.ThrowIfNull());
        Size = Math.Max(Count, Size);
    }
}
