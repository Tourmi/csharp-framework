using Microsoft.Xna.Framework;
using Tourmi.Monogame;

namespace Tourmi.Samples.TestScenes;

internal class NoiseGenerator
{
    private const float DefaultFrequency = 1.0f / 64f;

    private readonly Random _rand = new();

    private readonly PerlinNoise _noise = new();
    private readonly PerlinNoise[] _noises = [new(), new(), new(), new()];

    public Color[] SimpleGray(int width, int height)
    {
        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var value = _rand.Next(256);
                res[y * width + x] = new Color(value, value, value);
            }
        }

        return res;
    }

    public Color[] PerlinGray(int width, int height)
    {
        _noise.Shuffle();
        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var value = (_noise.Sample(x * DefaultFrequency, y * DefaultFrequency, width, height) + 1) / 2;
                res[y * width + x] = new Color(value, value, value);
            }
        }

        return res;
    }

    public Color[] PerlinAlpha(int width, int height)
    {
        _noise.Shuffle();
        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var value = (_noise.Sample(x * DefaultFrequency, y * DefaultFrequency, width, height) + 1) / 2;
                res[y * width + x] = new Color(Color.White, value);
            }
        }

        return res;
    }

    public Color[] FractalPerlinAlpha(int width, int height, int details = 4)
    {
        _noise.Shuffle();
        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var value = 0f;
                var amplitude = 1f;
                var freq = DefaultFrequency;

                for (var n = 0; n < details; n++)
                {
                    value += amplitude * _noise.Sample(
                        x * freq,
                        y * freq,
                        (int)MathF.Ceiling(width * freq),
                        (int)MathF.Ceiling(height * freq));
                    freq *= 2;
                    amplitude /= 2;
                }

                value = (value + 1) / 2;

                res[y * width + x] = new Color(Color.White, value);
            }
        }

        return res;
    }

    public Color[] FractalPerlinThresholds(int width, int height, int details = 4)
    {
        _noise.Shuffle();
        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var value = 0f;
                var amplitude = 1f;
                var freq = DefaultFrequency;

                for (var n = 0; n < details; n++)
                {
                    value += amplitude * _noise.Sample(
                        x * freq,
                        y * freq,
                        (int)MathF.Ceiling(width * freq),
                        (int)MathF.Ceiling(height * freq));
                    freq *= 2;
                    amplitude /= 2;
                }

                value = (value + 1) / 2;

                res[y * width + x] = value switch
                {
                    > 0.85f => Color.White,
                    > 0.75f => Color.Lerp(Color.DarkGray, Color.White, (value - 0.75f) / 0.1f),
                    > 0.5f => Color.Green,
                    > 0.47f => new Color(255, 240, 20),
                    > 0.40f => Color.Lerp(Color.Blue, Color.LightBlue, (value - 0.40f) / 0.07f),
                    > 0.25f => Color.Blue,
                    _ => Color.Lerp(Color.Black, Color.Blue, value / 0.25f),
                };
            }
        }

        return res;
    }

    public Color[] FractalPerlinHSVColors(int width, int height, int details = 4)
    {
        foreach (var noise in _noises)
        {
            noise.Shuffle();
        }

        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var values = new float[4];
                var amplitude = 1f;
                var freq = DefaultFrequency;

                for (var n = 0; n < details; n++)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] += amplitude * _noises[i].Sample(
                            x * freq,
                            y * freq,
                            (int)MathF.Ceiling(width * freq),
                            (int)MathF.Ceiling(height * freq));
                    }

                    freq *= 2;
                    amplitude /= 2;
                }

                res[y * width + x] = new Color(Color.FromHSV(values[0], 1 - MathF.Abs(values[1]), 1 - MathF.Abs(values[2])), 255);
            }
        }

        return res;
    }

    public Color[] FractalPerlinRGBColors(int width, int height, int details = 4)
    {
        foreach (var noise in _noises)
        {
            noise.Shuffle();
        }

        var res = new Color[width * height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var values = new float[4];
                var amplitude = 1f;
                var freq = DefaultFrequency;

                for (var n = 0; n < details; n++)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] += amplitude * _noises[i].Sample(
                            x * freq,
                            y * freq,
                            (int)MathF.Ceiling(width * freq),
                            (int)MathF.Ceiling(height * freq));
                    }

                    freq *= 2;
                    amplitude /= 2;
                }

                res[y * width + x] = new Color(1 - MathF.Abs(values[0]), 1 - MathF.Abs(values[1]), 1 - MathF.Abs(values[2]), 1f);
            }
        }

        return res;
    }
}
