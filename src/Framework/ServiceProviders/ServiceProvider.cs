using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Tourmi.Framework.ServiceProviders;

/// <inheritdoc/>
public sealed partial class ServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, object> _providers = [];

    /// <inheritdoc/>
    public void AddProvider<T>(T val)
    {
        if (_providers.ContainsKey(typeof(T)))
        {
            throw new ArgumentException($"Provider of type {typeof(T)} was already added");
        }

        _providers.Add(typeof(T), new InternalProvider<T, T>(val));
    }

    /// <inheritdoc/>
    public void AddProvider<T>(Func<T> func)
    {
        if (_providers.ContainsKey(typeof(T)))
        {
            throw new ArgumentException($"Provider of type {typeof(T)} was already added");
        }

        _providers.Add(typeof(T), new InternalProvider<T, T>(func));
    }

    /// <inheritdoc/>
    public void RemoveProvider<T>() => _ = _providers.Remove(typeof(T));

    ILazyProvider<T> IReadOnlyServiceProvider.GetProvider<T>() => (ILazyProvider<T>)GetProvider(typeof(T));

    private object GetProvider(Type type) => _providers.TryGetValue(type, out var value) ? value : throw new KeyNotFoundException($"No provider of type {type} exists");

    /// <inheritdoc/>
    [RequiresUnreferencedCode("Uses reflection")]
    public TClass Instantiate<TClass>() => (TClass)Instantiate(typeof(TClass));

    /// <inheritdoc/>
    [RequiresUnreferencedCode("Uses reflection")]
    public object Instantiate(Type type)
    {
        var constructor = type.ThrowIfNull().GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Single();
        var parameters = constructor.GetParameters();

        var args = new object[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            var currParamType = parameters[i].ParameterType;

            if (currParamType.IsGenericType && (currParamType.GetGenericTypeDefinition() == typeof(IReadOnlyLazyProvider<>) || currParamType.GetGenericTypeDefinition() == typeof(ILazyProvider<>)))
            {
                args[i] = GetProvider(currParamType.GenericTypeArguments[0]);
            }
            else
            {
                var provider = GetProvider(currParamType);
                var getMethod = typeof(IReadOnlyLazyProvider<>).MakeGenericType(provider.GetType().GenericTypeArguments[0]).GetMethod(nameof(IReadOnlyLazyProvider<object>.GetValue))!;
                args[i] = getMethod.Invoke(provider, null)!;
            }
        }

        return constructor.Invoke(args);
    }
}
