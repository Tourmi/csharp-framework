namespace Tourmi.Framework.ServiceProviders;

/// <summary>
/// A lazy provider which returns the registered <typeparamref name="T"/> service.
/// </summary>
public interface IReadOnlyLazyProvider<T>
{
    /// <summary>
    /// Returns the registered service, creating it if it does not exist yet
    /// </summary>
    /// <returns>the registered value</returns>
    T GetValue();
}
