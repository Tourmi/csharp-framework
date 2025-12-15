namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides a reference to a value of type <typeparamref name="T"/>.
/// Equivalent to a <see langword="ref"/> parameter.
/// </summary>
public ref struct Ref<T>(ref T reference)
{
    private ref T _reference = ref reference;

    /// <summary>
    /// Target of the reference.
    /// </summary>
    public ref T Reference => ref _reference;
}
