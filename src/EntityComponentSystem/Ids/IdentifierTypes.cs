namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// Represents the type of an identifier, and fits in the <see cref="Identifier.TypesBitMask"/> of the <see cref="Identifier"/>/<see cref="RelationComponentIdentifier"/>
/// </summary>
[Flags]
public enum IdentifierTypes : byte
{
    /// <summary>
    /// No special flags for the identifier
    /// </summary>
    None = 0,

    /// <summary>
    /// Whether or not a component should be automatically overridden 
    /// when extending an entity
    /// </summary>
    IsAutoOverride = 1 << 6,

    /// <summary>
    /// When set, this identifier is a <see cref="RelationComponentIdentifier"/>
    /// </summary>
    Relation = 1 << 7,
}
