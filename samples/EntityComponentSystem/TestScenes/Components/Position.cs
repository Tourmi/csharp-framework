using Microsoft.Xna.Framework;

namespace Tourmi.Samples.TestScenes.Components;

internal readonly record struct Position
{
    public Position()
    {
    }

    public Position(float x, float y)
    {
        Value = new(x, y);
    }

    public Vector2 Value { get; init; }

    public float X => Value.X;
    public float Y => Value.Y;
}
