namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides an instance of a component that is promised to be thread safe.
/// </summary>
/// <remarks>
/// <see cref="ThreadSafe{T}"/> does not mean that the type <typeparamref name="T"/> is thread safe,
/// but that access to the <see cref="Instance"/> will always be made in a threadsafe manner,
/// either because the type <typeparamref name="T"/> has built-in thread safety, or because only read access
/// to the instance is needed.
/// </remarks>
public readonly ref struct ThreadSafe<T>(T instance)
    where T : class
{
    /// <summary>
    /// Instance that is either thread safe, or only read from.
    /// </summary>
    public readonly T Instance { get; } = instance;
}
