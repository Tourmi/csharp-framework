using Microsoft.Xna.Framework;

namespace Tourmi.Samples.TestScenes;

internal class PerlinNoise
{
    private byte[] _permutations = [.. Enumerable.Repeat(Enumerable.Range(0, 256).Shuffle().Select(i => (byte)i).ToArray(), 2).SelectMany(i => i)];

    public void Shuffle()
    {
        _permutations = [.. Enumerable.Repeat(Enumerable.Range(0, 256).Shuffle().Select(i => (byte)i).ToArray(), 2).SelectMany(i => i)];
    }

    public float Sample(float x, float y, int xPeriod, int yPeriod)
    {
        var xInt0 = ((int)MathF.Floor(x) % xPeriod) & 0xff;
        var yInt0 = ((int)MathF.Floor(y) % yPeriod) & 0xff;
        var xInt1 = ((int)MathF.Floor(x + 1) % xPeriod) & 0xff;
        var yInt1 = ((int)MathF.Floor(y + 1) % yPeriod) & 0xff;
        var xFrac = x - MathF.Floor(x);
        var yFrac = y - MathF.Floor(y);

        var topLeft = Vector2.Dot(new Vector2(xFrac, yFrac), GetConstantVector(_permutations[_permutations[xInt0] + yInt0]));
        var topRight = Vector2.Dot(new Vector2(xFrac - 1, yFrac), GetConstantVector(_permutations[_permutations[xInt1] + yInt0]));
        var bottomLeft = Vector2.Dot(new Vector2(xFrac, yFrac - 1), GetConstantVector(_permutations[_permutations[xInt0] + yInt1]));
        var bottomRight = Vector2.Dot(new Vector2(xFrac - 1, yFrac - 1), GetConstantVector(_permutations[_permutations[xInt1] + yInt1]));

        var xSmooth = MathHelper.SmoothStep(0, 1, xFrac);
        var ySmooth = MathHelper.SmoothStep(0, 1, yFrac);

        return MathHelper.Lerp(MathHelper.Lerp(topLeft, topRight, xSmooth), MathHelper.Lerp(bottomLeft, bottomRight, xSmooth), ySmooth);
    }

    private Vector2 GetConstantVector(byte val)
    {
        return (val & 0b11) switch
        {
            0 => new(1, 1),
            1 => new(-1, 1),
            2 => new(-1, -1),
            _ => new(1, -1),
        };
    }
}
