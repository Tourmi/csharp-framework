#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Tourmi.Monogame.Helpers;

/// <summary>
/// Helper class for XNA vectors
/// </summary>
public static class Vectors
{
    public static Vector2 Create(float x, float y) => new(x, y);
    public static Vector3 Create(float x, float y, float z) => new(x, y, z);
    public static Vector3 Create(Vector2 xy, float z) => new(xy, z);
    public static Vector3 Create(float x, Vector2 yz) => new(x, yz.X, yz.Y);
    public static Vector4 Create(float x, float y, float z, float w) => new(x, y, z, w);
    public static Vector4 Create(Vector2 xy, float z, float w) => new(xy, z, w);
    public static Vector4 Create(float x, Vector2 yz, float w) => new(x, yz.X, yz.Y, w);
    public static Vector4 Create(float x, float y, Vector2 zw) => new(x, y, zw.X, zw.Y);
    public static Vector4 Create(Vector2 xy, Vector2 zw) => new(xy, zw.X, zw.Y);
    public static Vector4 Create(Vector3 xyz, float w) => new(xyz, w);
    public static Vector4 Create(float x, Vector3 yzw) => new(x, yzw.X, yzw.Y, yzw.Z);
}
