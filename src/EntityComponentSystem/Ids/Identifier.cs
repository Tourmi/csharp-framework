using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Tourmi.EntityComponentSystem.Queries;

namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// The unique identifier of an entity.
/// </summary>
/// <param name="id">Id of the entity</param>
[StructLayout(LayoutKind.Explicit, Size = 8)]
[SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "Comparison operators do not make sense")]
[DebuggerDisplay("{ShortId}, Types = {Types}, Version = {Version}")]
public readonly struct Identifier(ulong id) : IEquatable<Identifier>, IComparable<Identifier>, IQueryParam<Identifier>
{
    /// <summary>
    /// Comparer that allows the comparison of two collections of identifiers
    /// </summary>
    public static readonly IComparer<Identifier[]> IdentifierCollectionComparer = Comparer.FromFunc((Identifier[] c1, Identifier[] c2) =>
    {
        var compare = c1.Length.CompareTo(c2.Length);
        if (compare != 0)
        {
            return compare;
        }

        for (var i = 0; i < c1.Length; i++)
        {
            compare = c1[i].CompareTo(c2[i]);
            if (compare != 0)
            {
                return compare;
            }
        }

        return 0;
    });

    /// <summary>
    /// Mask for the short form of the identifier.
    /// </summary>
    public const ulong IdBitMask = 0x0000_0000_FFFF_FFFF;

    /// <summary>
    /// Mask for the version (also known as generation) of the entity.
    /// </summary>
    public const ulong VersionBitMask = 0x0000_FFFF_0000_0000;

    /// <summary>
    /// Offset in bits where the version value starts.
    /// </summary>
    public const byte VersionBitOffset = 32;

    /// <summary>
    /// Mask for the type of the entity.
    /// </summary>
    public const ulong TypesBitMask = 0xFF00_0000_0000_0000;

    /// <summary>
    /// Offset in bits for where the Type value starts.
    /// </summary>
    public const byte TypesBitOffset = 56;

    /// <summary>
    /// Value of the id of the entity
    /// </summary>
    [field: FieldOffset(0)]
    public ulong Value { get; } = id;

    /// <summary>
    /// Short form of the identifier.
    /// </summary>
    [field: FieldOffset(0)]
    public uint ShortId { get; }

    /// <summary>
    /// Version of the identifier.
    /// </summary>
    [field: FieldOffset(4)]
    public ushort Version { get; }

    /// <summary>
    /// Type flags of the identifier
    /// </summary>
    [field: FieldOffset(7)]
    public IdentifierTypes Types { get; }

    /// <summary>
    /// Whether or not this Id is a <see cref="RelationComponentIdentifier"/>
    /// </summary>
    public bool IsRelationId => Types.HasFlag(IdentifierTypes.Relation);

    /// <summary>
    /// Implicitely converts the identifier to a ulong.
    /// </summary>
    public static implicit operator ulong(Identifier identifier) => identifier.Value;

    /// <summary>
    /// Implicitely converts the ulong to an identifier.
    /// </summary>
    public static implicit operator Identifier(ulong id) => new(id);

    /// <inheritdoc/>
    public static bool operator ==(Identifier left, Identifier right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Identifier left, Identifier right) => !(left == right);

    /// <summary>
    /// Returns a new identifier with the version number incremented.
    /// </summary>
    public Identifier IncrementVersion()
    {
        var version = Version;
        unchecked
        {
            // intentional overflow
            version++;
        }

        return Value & ~VersionBitMask | (ulong)version << VersionBitOffset;
    }

    /// <summary>
    /// Converts this id into a <see cref="RelationComponentIdentifier"/>
    /// </summary>
    public RelationComponentIdentifier ToRelationId() => new(this);

    /// <inheritdoc/>
    public bool Equals(Identifier other) => Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Identifier other && other.Value == Value;

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc/>
    public int CompareTo(Identifier other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    public override string ToString() => $"{{ShortId = {ShortId:x}, Version = {Version}, Types = {Types}}}";

    static Identifier IQueryParam<Identifier>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex]);
}
