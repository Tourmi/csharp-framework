using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Collections;

/// <summary>
/// Pools items of type <typeparamref name="T"/>. This class is not thread-safe.
/// New items must be added via <see cref="Push(T)"/>.
/// </summary>
public class Pool<T> : IPool<T>
    where T : class
{
    private readonly Stack<T> _items = [];

    /// <inheritdoc/>
    public int Count => _items.Count;

    /// <inheritdoc/>
    public int Size { get; private set; }

    /// <inheritdoc/>
    public T Pop() => TryPop(out var item) ? item : throw new InvalidOperationException("The pool is exhausted, cannot Pop an item.");

    /// <inheritdoc/>
    public bool TryPop([NotNullWhen(true)] out T? item) => _items.TryPop(out item);

    /// <inheritdoc/>
    public void Push(T item)
    {
        _items.Push(item.ThrowIfNull());
        Size++;
    }
}
