namespace Tourmi.EntityComponentSystem.Attributes;

/// <summary>
/// Marks a struct or class as a relationship type.
/// A fixed relation type value greater than 255 can be passed in via <paramref name="relationType"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class ComponentRelationTypeAttribute(ushort relationType = 0) : Attribute
{
    /// <summary>
    /// Whether or not the relation is unique, meaning another relation of the same type cannot be added twice to the same entity.
    /// </summary>
    public bool IsUnique { get; set; }

    /// <summary>
    /// Whether or not the relation is symmetric. 
    /// ie: Adding (AtWarWith, X) to entity Y will also add (AtWarWith, Y) to X.
    /// </summary>
    public bool IsSymmetric { get; set; }

    /// <summary>
    /// Whether or not the relation is Reflective. 
    /// ie: HasRelation(IsA, Car) will return <see langword="true"/> for the Car entity, 
    /// even though it does not contain the Relation{IsA, Car} component.
    /// </summary>
    public bool IsReflective { get; set; }

    /// <summary>
    /// Whether or not the relation is transitive.
    /// ie: If <c>Car.IsA(MotorVehicule)</c> and <c>MotorVehicule.IsA(Vehicule)</c> are both <see langword="true"/>, 
    /// then <c>Car.IsA(Vehicule)</c> will be <see langword="true"/> too.
    /// </summary>
    public bool IsTransitive { get; set; }

    /// <summary>
    /// Type of the relation definition. Values below 256 are reserved.
    /// If 0, the relation type id will be auto-generated.
    /// </summary>
    public ushort RelationType { get; } = relationType;
}
