using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Tourmi.EntityComponentSystem.Relations;

/// <summary>
/// The unique identifier of a relationship component.
/// </summary>
/// <param name="id">Id of the entity</param>
[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct RelationComponentIdentifier(ulong id) : IEquatable<RelationComponentIdentifier>
{
    /// <summary>
    /// Mask for the <see cref="Target"/> of a component
    /// </summary>
    public const ulong TargetBitMask = 0x0000_0000_FFFF_FFFF;

    /// <summary>
    /// Mask for the <see cref="RelationType"/> this component has with its target
    /// </summary>
    public const ulong RelationTypeBitMask = 0x0000_FFFF_0000_0000;

    /// <summary>
    /// Offset in bits where the <see cref="RelationType"/> value starts.
    /// </summary>
    public const int RelationTypeBitOffset = 32;

    /// <summary>
    /// Mask for the types of the component.
    /// </summary>
    public const ulong TypesBitMask = 0xFF00_0000_0000_0000;

    /// <summary>
    /// Offset in bits for where the <see cref="Types"/> value starts.
    /// </summary>
    public const int TypesBitOffset = 56;

    /// <summary>
    /// Creates a relation identifier from the given parameters
    /// </summary>
    /// <param name="target">Target of the relationship</param>
    /// <param name="relationType">Type of the relation</param>
    /// <param name="identifierTypes">Type flags for the identifier</param>
    public RelationComponentIdentifier(uint target, ushort relationType, byte identifierTypes = (byte)IdentifierTypes.Relation)
        : this(target | ((ulong)relationType << RelationTypeBitOffset) | ((ulong)(identifierTypes | (byte)IdentifierTypes.Relation) << TypesBitOffset)) { }

    /// <inheritdoc cref="RelationComponentIdentifier(uint, ushort, byte)"/>
    public RelationComponentIdentifier(uint target, ushort relationType, IdentifierTypes identifierTypes = IdentifierTypes.Relation)
        : this(target, relationType, (byte)identifierTypes) { }

    /// <inheritdoc cref="RelationComponentIdentifier(uint, ushort, IdentifierTypes)"/>
    public RelationComponentIdentifier(uint target, ushort relationType)
        : this(target, relationType, IdentifierTypes.Relation) { }

    /// <inheritdoc cref="RelationComponentIdentifier(uint, ushort, IdentifierTypes)"/>
    public RelationComponentIdentifier(uint target, BuiltInRelationType relationType)
        : this(target, (ushort)relationType, IdentifierTypes.Relation) { }

    /// <summary>
    /// Value of the id of the component
    /// </summary>
    [field: FieldOffset(0)]
    public ulong Value { get; } = id;

    /// <summary>
    /// Target of the relationship.
    /// </summary>
    [field: FieldOffset(0)]
    public uint Target { get; }

    /// <summary>
    /// Relation type of the identifier.
    /// </summary>
    [field: FieldOffset(4)]
    public ushort RelationType { get; }

    /// <summary>
    /// Type flags of the identifier
    /// </summary>
    [field: FieldOffset(7)]
    public IdentifierTypes Types { get; }

    /// <summary>
    /// Shortcut property mapping the <see cref="RelationType"/> to the appropriate <see cref="BuiltInRelationType"/>.
    /// Will return <see langword="null"/> if the relation type cannot be mapped to a built-in type.
    /// </summary>
    public BuiltInRelationType? BuiltInRelationTypeOrNull
    {
        get
        {
            var relationType = RelationType;
            if (relationType >= FixedIds.Relations.RegionSize)
            {
                return null;
            }

            return (BuiltInRelationType)relationType;
        }
    }

    /// <summary>
    /// Implicitely converts the identifier to a ulong.
    /// </summary>
    public static implicit operator ulong(RelationComponentIdentifier identifier) => identifier.Value;

    /// <summary>
    /// Implicitely converts the ulong to an identifier.
    /// </summary>
    public static implicit operator RelationComponentIdentifier(ulong id) => new(id);

    /// <inheritdoc/>
    public static bool operator ==(RelationComponentIdentifier left, RelationComponentIdentifier right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(RelationComponentIdentifier left, RelationComponentIdentifier right) => !(left == right);

    /// <inheritdoc/>
    public bool Equals(RelationComponentIdentifier other) => Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is RelationComponentIdentifier other && other.Value == Value;

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();
}
