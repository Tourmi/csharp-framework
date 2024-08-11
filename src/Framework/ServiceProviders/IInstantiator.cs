using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.ServiceProviders;

/// <summary>
/// Allows instantiation of generic objects, or via reflection, and supports dependency injection
/// </summary>
public interface IInstantiator
{
    /// <summary>
    /// Instantiates the given type with the registered services. Fails when a class has more than one constructor
    /// </summary>

    [RequiresUnreferencedCode("Uses reflection")]
    T Instantiate<T>();
    /// <summary>
    /// Instantiates the given type with the registered services. Fails when a class has more than one constructor
    /// </summary>

    [RequiresUnreferencedCode("Uses reflection")]
    object Instantiate(Type type);
}
