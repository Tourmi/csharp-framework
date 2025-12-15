namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Marks a query parameter as threadsafe. 
/// <para/>Should be used when only read access to a type is needed, and the type is not a value-type.
/// <para/>Should not be used if write access to the parameter is needed, and it is not a thread-safe type.
/// <para/>Will do nothing on value type parameters, or if a <see langword="ref"/>/<see langword="out"/> parameter is requested.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public sealed class ThreadSafeAttribute : Attribute;
