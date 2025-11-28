namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Sets the type of an enum entry to a specific component.
/// </summary>
[AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
internal sealed class BackingComponentTypeAttribute(Type componentType) : Attribute
{
    /// <summary>
    /// The type associated to the enum entry.
    /// </summary>
    public Type ComponentType { get; } = componentType;
}
