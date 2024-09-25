#nullable disable
namespace Tourmi.RLMV.Aseprite;

/// <summary>
/// Metadata for an individual animation in the sprite
/// </summary>
public class FrameTag
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
    /// Color of the frame as an hexadecimal string. ie: #fe5b59ff
    /// </summary>
    public string Color { get; set; }
    /// <summary>
    /// Additional data defined in the tag
    /// </summary>
    public string Data { get; set; }
}
