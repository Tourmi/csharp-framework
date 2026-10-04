namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// Represents a reserved identifier region for an ecs <see cref="World"/>.
/// </summary>
public sealed class IdentifierRegion
{
    /// <summary>
    /// If specified, Id at which this region of identifiers must start.
    /// If 0, will automatically be set to the last available Id upon reservation, as to avoid leaving gaps in entity Ids.
    /// Cannot have a value such as <see cref="Offset"/> + <see cref="Amount"/> would give a value higher than <see cref="uint.MaxValue"/>.
    /// </summary>
    /// <remarks>
    /// Once set, cannot be modified.
    /// </remarks>
    public uint Offset
    {
        get;
        set
        {
            if (field != 0)
            {
                throw new InvalidOperationException("Cannot set Offset once it has already been set.");
            }

            field = value.ThrowIfGreaterThan(uint.MaxValue - Amount);
        }
    }

    /// <summary>
    /// Minimum amount of identifiers that should be included in this region.
    /// If 0, reserving the region will do nothing.
    /// Cannot have a value such as <see cref="Offset"/> + <see cref="Amount"/> would give a value higher than <see cref="uint.MaxValue"/>
    /// </summary>
    public required uint Amount
    {
        get;
        init => field = Offset is 0 ? value : value.ThrowIfGreaterThan(uint.MaxValue - Offset + 1);
    }

    /// <summary>
    /// Name of the region, useful for debugging.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Id at which the region ends, inclusive. Undefined if <see cref="Offset"/> or <see cref="Amount"/> is 0.
    /// </summary>
    private uint EndIdInclusive => Offset + (Amount - 1);

    /// <summary>
    /// Returns <see langword="true"/> if this region overlaps with the <paramref name="other"/>
    /// </summary>
    public bool OverlapsWith(IdentifierRegion other)
    {
        _ = other.ThrowIfNull();

        if (Offset == 0)
        {
            return false;
        }

        if (other.Offset == 0)
        {
            return false;
        }

        return Offset <= other.EndIdInclusive && EndIdInclusive >= other.Offset;
    }

    /// <summary>
    /// Returns <see langword="true"/> if this region contains the given <paramref name="id"/>.
    /// </summary>
    public bool Contains(Identifier id) => Offset <= id.ShortId && id.ShortId <= EndIdInclusive;

    /// <inheritdoc/>
    public override string ToString() => $"IdentifierRegion: {{ Name: '{Name ?? "Unnamed"}', Offset: {Offset}, Amount: {Amount} }}";
}
