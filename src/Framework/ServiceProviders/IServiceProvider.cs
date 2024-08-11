namespace Tourmi.Framework.ServiceProviders;

/// <summary>
/// Allows the registration and fetching of different services. Note that all services are singletons
/// </summary>
public interface IServiceProvider : IReadOnlyServiceProvider, IInstantiator
{
    /// <summary>
    /// Adds a provider with the given value.
    /// </summary>
    void AddProvider<T>(T val);

    /// <summary>
    /// Adds a provider with the given factory method
    /// </summary>
    void AddProvider<T>(Func<T> func);

    /// <summary>
    /// Removes the provider of the given type if it exists
    /// </summary>
    void RemoveProvider<T>();
}
