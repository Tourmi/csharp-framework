using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tourmi.Samples.TestScenes.Components;

internal sealed class Sprite
{
    public Texture2D? Texture { get; set; }

    public Rectangle DestinationRectangle { get; set; }

    public Rectangle? SourceRectangle { get; set; }

    public Color Color { get; set; } = Color.White;

    public float Rotation { get; set; }

    public Vector2 Origin { get; set; }

    public SpriteEffects SpriteEffects { get; set; }
}
