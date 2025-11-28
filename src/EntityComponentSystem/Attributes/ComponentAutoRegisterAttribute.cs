namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a type as a component that will be automatically created into new <see cref="World"/> instances.
/// The type will be assumed to store data unless otherwise specified with <see cref="ComponentFlagAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ComponentAutoRegisterAttribute : Attribute;
