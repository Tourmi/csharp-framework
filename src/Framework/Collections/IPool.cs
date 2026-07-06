using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Collections;

/// <summary>
/// Pools items of type <typeparamref name="T"/>.
/// </summary>
internal interface IPool<T>
    where T : class
{
    /// <summary>
    /// Attempts to take an item from the pool, returning false if the pool has been exhausted.
    /// </summary>
    bool TryTake([NotNullWhen(true)] out T? item);

    /// <summary>
    /// Returns the <paramref name="item"/> to the pool, and sets the original reference to null.
    /// </summary>
    void Return(T item);
}
