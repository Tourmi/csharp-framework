using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Collections.Generic;

/// <summary>
/// Pools items of type <typeparamref name="T"/>. This class is not thread-safe.
/// </summary>
public class Pool<T> : IPool<T>
    where T : class
{
    private readonly Stack<T> _items = [];

    /// <inheritdoc/>
    public bool TryTake([NotNullWhen(true)] out T? item) => _items.TryPop(out item);

    /// <inheritdoc/>
    public void Return(T instance) => _items.Push(instance.ThrowIfNull());
}
