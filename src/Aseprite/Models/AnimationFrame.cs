#nullable disable
namespace Tourmi.RLMV.Aseprite;

/// <summary>
/// Represents a single animation frame.
/// </summary>
public class AnimationFrame
{
    /// <summary>
    /// Custom name for this frame
    /// </summary>
    public string Filename { get; set; }
    /// <summary>
    /// The zone on the original texture image to render for this frame.
    /// </summary>
    public Rectangle Frame { get; set; }
    /// <summary>
    /// Whether or not this frame is rotated.
    /// </summary>
    public bool Rotated { get; set; }
    /// <summary>
    /// Whether or not this frame was trimmed
    /// </summary>
    public bool Trimmed { get; set; }
    /// <summary>
    /// The sprite's source size, usage unknown
    /// </summary>
    public Rectangle SpriteSourceSize { get; set; }
    /// <summary>
    /// The frame's source size? Usage unknown
    /// </summary>
    public Rectangle SourceSize { get; set; }
    /// <summary>
    /// Frame duration in miliseconds 
    /// </summary>
    public int Duration { get; set; }
}
