namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a component as requiring another.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class RequiresAttribute(Type requiredComponentType) : Attribute
{
    /// <summary>
    /// Component type required by the component
    /// </summary>
    public Type RequiredComponentType { get; } = requiredComponentType;
}
