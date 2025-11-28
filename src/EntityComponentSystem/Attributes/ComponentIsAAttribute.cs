namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a component with the <see cref="IsA"/> relationship, targeting another type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ComponentIsAAttribute(Type targetType) : Attribute
{
    /// <summary>
    /// The target component of the <see cref="IsA"/> relationship.
    /// </summary>
    public Type TargetType { get; } = targetType;
}
