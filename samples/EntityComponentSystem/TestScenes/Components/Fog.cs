using Microsoft.Xna.Framework;

namespace Tourmi.Samples.TestScenes.Components;

internal readonly struct Fog
{
    public Vector2 Position { get; init; }
    public Vector2 Wind { get; init; }
    public Vector2 MinWrapAround { get; init; }
}
