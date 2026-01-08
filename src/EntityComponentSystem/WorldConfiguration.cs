namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Configuration used to instantiate an ecs <see cref="World"/>.
/// </summary>
public sealed class WorldConfiguration
{
    /// <summary>
    /// Regions of identifiers that are reserved for special purposes.
    /// Default entity generation will never generate entities with an Id contained in these regions.
    /// </summary>
    public IEnumerable<IdentifierRegion> ReservedIdentifierRegions
    {
        get => field ?? [];
        set => field = value ?? [];
    }
}
