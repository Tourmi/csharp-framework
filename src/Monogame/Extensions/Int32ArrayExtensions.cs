namespace Tourmi.Monogame.Extensions;

/// <summary>
/// Extension methods for an <see cref="int"/><c>[]</c>
/// </summary>
public static class Int32ArrayExtensions
{
    /// <summary>
    /// Turns an int array into a Point
    /// </summary>
    /// <param name="arr"></param>
    /// <returns>The new point</returns>
    public static Point ToPoint(this int[] arr) => new(arr.ThrowIfNull()[0], arr[1]);
}
