#pragma warning disable CA1819 // Properties should not return arrays
#nullable disable

namespace Tourmi.RLMV.Aseprite;

/// <summary>
/// Class representing a single aseprite export.
/// </summary>
public class Sprite
{
    /// <summary>
    /// All the animation frames for this sprite
    /// </summary>
    public AnimationFrame[] Frames { get; set; }

    /// <summary>
    /// Metadata for this sprite
    /// </summary>
    public Meta Meta { get; set; }
}
