using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// The unique identifier of an entity.
/// </summary>
/// <param name="id">Id of the entity</param>
[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct Identifier(ulong id) : IEquatable<Identifier>, IComparable<Identifier>
{
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

    /// <inheritdoc/>
    public bool Equals(Identifier other) => Value == other.Value;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Identifier other && other.Value == Value;

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc/>
    public int CompareTo(Identifier other) => Value.CompareTo(other.Value);
}
