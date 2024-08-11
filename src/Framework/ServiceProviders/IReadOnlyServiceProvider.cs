namespace Tourmi.Framework.ServiceProviders;

/// <summary>
/// Readonly service provider, that does not allow the registration of new services
/// </summary>
public interface IReadOnlyServiceProvider
{
    internal ILazyProvider<T> GetProvider<T>();
}
