#pragma warning disable CA1819 // Properties should not return arrays
#nullable disable

namespace Tourmi.RLMV.Aseprite;

/// <summary>
/// Meta data for the Sprite
/// </summary>
public class Meta
{
    /// <summary>
    /// Version sprite was exported with
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Image filename for this Sprite
    /// </summary>
    public string Image { get; set; }
    /// <summary>
    /// Image format string for the sprite
    /// </summary>
    public string Format { get; set; }
    /// <summary>
    /// Size of the image file
    /// </summary>
    public Rectangle Size { get; set; }
    /// <summary>
    /// Scale string
    /// </summary>
    public string Scale { get; set; }
    /// <summary>
    /// Frame tags of the sprite
    /// </summary>
    public FrameTag[] FrameTags { get; set; }
}
