namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a struct or class as a component that will be automatically registered into new <see cref="World"/> instances.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class AutoRegisterComponentAttribute : Attribute;
