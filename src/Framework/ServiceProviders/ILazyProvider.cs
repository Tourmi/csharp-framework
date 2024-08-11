namespace Tourmi.Framework.ServiceProviders;

/// <summary>
/// A lazy provider which returns the registered <typeparamref name="T"/> service.
/// It also supports directly setting the internal value.
/// </summary>
public interface ILazyProvider<T> : IReadOnlyLazyProvider<T>
{
    /// <summary>
    /// Sets the value stored in the provider to the given value
    /// </summary>
    /// <param name="value">The value to set to</param>
    void SetValue(T value);
}
