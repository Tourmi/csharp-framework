namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a type as a component that will be automatically created into new <see cref="World"/> instances.
/// The type will be assumed to store data unless otherwise specified with <see cref="TagComponentAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class AutoRegisterComponentAttribute : Attribute;
