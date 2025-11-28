namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a struct or class as a flag component (does not contain data).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ComponentFlagAttribute : Attribute;
