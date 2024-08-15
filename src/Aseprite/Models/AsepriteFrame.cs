#nullable disable
namespace Tourmi.RLMV.Aseprite;

/// <summary>
/// Represents a single animation frame.
/// </summary>
public class AsepriteFrame
{
    /// <summary>
    /// Custom name for this frame
    /// </summary>
    public string Filename { get; set; }
    /// <summary>
    /// The zone on the original texture image to render for this frame.
    /// </summary>
    public AsepriteRectangle Frame { get; set; }
    /// <summary>
    /// Whether or not this frame is rotated.
    /// </summary>
    public bool Rotated { get; set; }
    /// <summary>
    /// Whether or not this frame was trimmed
    /// </summary>
    public bool Trimmed { get; set; }
    /// <summary>
    /// Frame duration in miliseconds 
    /// </summary>
    public int Duration { get; set; }
}
