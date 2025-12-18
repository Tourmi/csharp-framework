namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query parameter that provides a write-only reference to a value of type <typeparamref name="T"/>.
/// Equivalent to the <see langword="out"/> keyword.
/// </summary>
public readonly ref struct OutRef<T>(ref T reference)
{
    private readonly ref T _reference = ref reference;

    /// <summary>
    /// Sets the value of the reference.
    /// </summary>
    public void SetValue(T value) => _reference = value;
}
