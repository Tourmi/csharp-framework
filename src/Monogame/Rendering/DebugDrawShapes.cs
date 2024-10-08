﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Monogame.Rendering;

/// <summary>
/// Shape drawing utility. 
/// Should only be used for debugging, as these functions are extremely inefficient. 
/// The Texture2D `whitePixel` needs to be a 1x1 white pixel.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Debug utilities, no need for validation")]
public static class DebugDrawShapes
{
    public static void DrawRectangle(Texture2D whitePixel, SpriteBatch batch, Rectangle area, int width, Color color)
    {
        batch.Draw(whitePixel, new Rectangle(area.X, area.Y, area.Width, width), color);
        batch.Draw(whitePixel, new Rectangle(area.X, area.Y, width, area.Height), color);
        batch.Draw(whitePixel, new Rectangle(area.X + area.Width - width, area.Y, width, area.Height), color);
        batch.Draw(whitePixel, new Rectangle(area.X, area.Y + area.Height - width, area.Width, width), color);
    }

    public static void DrawRectangle(Texture2D whitePixel, SpriteBatch spriteBatch, Rectangle area)
    {
        DrawRectangle(whitePixel, spriteBatch, area, 1, Color.White);
    }

    public static void DrawFullCircle(Texture2D whitePixel, SpriteBatch spritebatch, Vector2 center, float radius, Color color, int segments = 64)
    {
        DrawCircle(whitePixel, spritebatch, center, radius, color, (int)radius + 1, segments);
    }

    public static void DrawFadingCircle(Texture2D whitePixel, SpriteBatch spritebatch, Vector2 center, float radius, Color color, int lineWidth = 1, int segments = 64, int fadeRadius = 5)
    {
        var currentRadius = 1;
        var maxFadeLevel = fadeRadius + 4;
        DrawCircle(whitePixel, spritebatch, center, radius - currentRadius, new Color(color, 255), 1, segments);

        while (currentRadius < fadeRadius)
        {
            DrawCircle(whitePixel, spritebatch, center, radius - currentRadius, new Color(color, 255 - (currentRadius + 4) * (255 / maxFadeLevel)), 1, segments);
            currentRadius++;
        }
    }

    public static void DrawCircle(Texture2D whitePixel, SpriteBatch spritebatch, Vector2 center, float radius, Color color, int lineWidth = 2, int segments = 64)
    {
        var vertex = new Vector2[segments];

        var increment = Math.PI * 2.0 / segments;
        var theta = 0.0;

        for (var i = 0; i < segments; i++)
        {
            vertex[i] = center + radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
            theta += increment;
        }

        DrawPolygon(whitePixel, spritebatch, vertex, segments, color, lineWidth);
    }

    public static void DrawPolygon(Texture2D whitePixel, SpriteBatch spriteBatch, Vector2[] vertex, int count, Color color, int lineWidth)
    {
        if (count > 0)
        {
            for (var i = 0; i < count - 1; i++)
            {
                DrawLineSegment(whitePixel, spriteBatch, vertex[i], vertex[i + 1], color, lineWidth);
            }

            DrawLineSegment(whitePixel, spriteBatch, vertex[count - 1], vertex[0], color, lineWidth);
        }
    }

    public static void DrawLineSegment(Texture2D whitePixel, SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
    {
        var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        var length = Vector2.Distance(point1, point2);

        spriteBatch.Draw(whitePixel, point1, null, color,
        angle, Vector2.Zero, new Vector2(length, lineWidth),
        SpriteEffects.None, 0f);
    }
}
