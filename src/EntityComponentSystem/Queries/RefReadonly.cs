namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides a readonly reference for a value of type <typeparamref name="T"/>. 
/// Equivalent to the <see langword="in"/> or <see langword="ref"/> <see langword="readonly"/> keywords.
/// </summary>
public readonly ref struct RefReadonly<T>(ref readonly T reference)
{
    private readonly ref readonly T _reference = ref reference;

    /// <summary>
    /// Target of the reference
    /// </summary>
    public readonly ref readonly T Reference => ref _reference;
}
