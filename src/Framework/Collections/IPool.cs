using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Collections;

/// <summary>
/// Pools items of type <typeparamref name="T"/>.
/// </summary>
public interface IPool<T>
    where T : class
{
    /// <summary>
    /// The amount of items left in the pool
    /// </summary>
    int Count { get; }

    /// <summary>
    /// The amount of items that this pool would have, if all its items are pushed back into it.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// Takes an items from the pool. This may throw an <see cref="InvalidOperationException"/> depending on the pool implementation.
    /// </summary>
    T Pop();

    /// <summary>
    /// Attempts to take an item from the pool, returning false if the pool has been exhausted.
    /// </summary>
    bool TryPop([NotNullWhen(true)] out T? item);

    /// <summary>
    /// Pushes the <paramref name="item"/> back into the pool.
    /// </summary>
    void Push(T item);
}
