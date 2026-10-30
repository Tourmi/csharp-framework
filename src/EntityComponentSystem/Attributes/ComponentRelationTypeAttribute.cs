namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a struct or class as a relationship type.
/// A fixed relation type value greater than 255 can be passed in via <paramref name="relationType"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class ComponentRelationTypeAttribute(ushort relationType = 0) : Attribute
{
    /// <summary>
    /// Whether or not the relation is unique, meaning another relation of the same type cannot be added twice to the same entity.
    /// </summary>
    public bool IsUnique { get; set; }

    /// <summary>
    /// Type of the relation definition. Values below 256 are reserved.
    /// If 0, the relation type id will be auto-generated.
    /// </summary>
    public ushort RelationType { get; } = relationType;
}
