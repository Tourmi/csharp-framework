#nullable disable
namespace Tourmi.RLMV.Aseprite;

/// <summary>
/// Metadata for an individual animation in the sprite
/// </summary>
public class AsepriteFrameTag
{
    /// <summary>
    /// Name for the animation
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Starting index for the animation in the Frames array
    /// </summary>
    public int From { get; set; }
    /// <summary>
    /// End index for the animation in the Frames array (inclusive)
    /// </summary>
    public int To { get; set; }
    /// <summary>
    /// The direction to read the animation in. Possible values: forward, backward, ping-pong
    /// </summary>
    public string Direction { get; set; }
    /// <summary>
    /// Additional data defined in the tag
    /// </summary>
    public string Data { get; set; }
}
