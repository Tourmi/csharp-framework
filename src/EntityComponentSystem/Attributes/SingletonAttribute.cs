namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a struct or class as a Singleton component (component contains its own data).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class SingletonAttribute : Attribute;
