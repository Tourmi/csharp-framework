namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a struct or class as a relationship type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class ComponentRelationTypeAttribute : Attribute
{
    /// <summary>
    /// Whether or not the relation is unique, meaning another relation of the same type cannot be added twice to the same entity.
    /// </summary>
    public bool Unique { get; set; }
}
