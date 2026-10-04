namespace Tourmi.Samples.TestScenes;

/// <summary>
/// Static
/// </summary>
internal static class Events
{
    internal readonly record struct PreDraw;
    internal readonly record struct Draw;
    internal readonly record struct PreDrawUI;
    internal readonly record struct DrawUI;
    internal readonly record struct PostDrawUI;
    internal readonly record struct PostDraw;
}
