namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// Represents a reserved identifier region for an ecs <see cref="World"/>.
/// </summary>
public class IdentifierRegion
{
    private uint _offset;
    private uint _amount;

    /// <summary>
    /// If specified, Minimum Id at which this region of identifiers must start.
    /// Cannot be 0, and cannot have a value such as <see cref="Offset"/> + <see cref="Amount"/> would give a value higher than <see cref="uint.MaxValue"/>
    /// </summary>
    public required uint Offset
    {
        get => _offset;
        init => _offset = value.ThrowIfZero().ThrowIfGreaterThan(uint.MaxValue - Amount);
    }

    /// <summary>
    /// Minimum amount of identifiers that should be included in this region.
    /// Cannot be 0, and cannot have a value such as <see cref="Offset"/> + <see cref="Amount"/> would give a value higher than <see cref="uint.MaxValue"/>
    /// </summary>
    public required uint Amount
    {
        get => _amount;
        init => _amount = value.ThrowIfZero().ThrowIfGreaterThan(uint.MaxValue - Offset);
    }

    /// <summary>
    /// Name of the region, useful for debugging.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Id at which the region ends, inclusive.
    /// </summary>
    public uint EndIdInclusive => checked(Offset + (Amount - 1));

    /// <summary>
    /// Returns true if this region overlaps with the <paramref name="other"/>
    /// </summary>
    public bool OverlapsWith(IdentifierRegion other)
    {
        _ = other.ThrowIfNull();

        return Offset <= other.EndIdInclusive && EndIdInclusive >= other.Offset;
    }

    /// <inheritdoc/>
    public override string ToString() => $"IdentifierRegion: {{ Name: '{Name ?? "Unnamed"}', Offset: {Offset}, Amount: {Amount}, EndIdInclusive: {EndIdInclusive} }}";
}
