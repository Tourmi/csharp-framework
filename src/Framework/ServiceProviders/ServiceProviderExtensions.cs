using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.ServiceProviders;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/>
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Adds a provider which will be automatically instantiated when needed
    /// </summary>
    [RequiresUnreferencedCode("Uses reflection")]
    public static void AddProvider<TTarget, TClass>(this IServiceProvider serviceProvider)
        where TClass : TTarget
    {
        serviceProvider.ThrowIfNull().AddProvider<TTarget>(() => serviceProvider.Instantiate<TClass>());
    }

    /// <summary>
    /// Adds a new provider that will provide a value of type <typeparamref name="T"/>
    /// </summary>
    /// <param name="serviceProvider">This service provider</param>
    [RequiresUnreferencedCode("Uses reflection")]
    public static void AddProvider<T>(this IServiceProvider serviceProvider)
        => serviceProvider.ThrowIfNull().AddProvider(serviceProvider.Instantiate<T>);

    /// <summary>
    /// Adds a provider that uses the given <paramref name="factoryFunc"/> to instantiate the initial value once needed
    /// </summary>
    public static void AddProvider<TTarget, TClass>(this IServiceProvider serviceProvider, Func<IReadOnlyServiceProvider, TClass> factoryFunc)
        where TClass : TTarget
        => serviceProvider.ThrowIfNull().AddProvider<TTarget>(() => factoryFunc(serviceProvider));

    /// <summary>
    /// Adds a lazy provider of type <typeparamref name="TLazyProvider"/> which will immediately be instantiated
    /// </summary>
    [RequiresUnreferencedCode("Uses reflection")]
    public static void AddLazyProvider<TTarget, TLazyProvider>(this IServiceProvider serviceProvider)
        where TLazyProvider : IReadOnlyLazyProvider<TTarget>
    {
        serviceProvider.ThrowIfNull().AddProvider(serviceProvider.Instantiate<TLazyProvider>().GetValue);
    }

    /// <summary>
    /// Reuses the provider for <typeparamref name="TExisting"/> as <typeparamref name="TTarget"/>.
    /// Useful if <typeparamref name="TExisting"/> implements multiple interfaces
    /// </summary>
    public static void ReuseProvider<TTarget, TExisting>(this IServiceProvider serviceProvider)
        where TExisting : TTarget
    {
        serviceProvider.ThrowIfNull().AddProvider<TTarget>(() => serviceProvider.GetProvider<TExisting>().GetValue());
    }

    /// <summary>
    /// Sets the value for the given type of provider
    /// </summary>
    public static void SetValue<T>(this IServiceProvider serviceProvider, T val)
        => serviceProvider.ThrowIfNull().GetProvider<T>().SetValue(val);

    /// <summary>
    /// returns the stored value of a provider
    /// </summary>
    /// <returns>stored value of a provider</returns>
    public static T GetValue<T>(this IReadOnlyServiceProvider serviceProvider)
        => serviceProvider.ThrowIfNull().GetProvider<T>().GetValue();
}
