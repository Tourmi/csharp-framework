namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/>
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Orders the given <paramref name="enumerable"/> randomly
    /// </summary>
    /// <typeparam name="T">Type of the items in the enumerable</typeparam>
    /// <param name="enumerable">The enumerable to order randomly</param>
    /// <param name="random">The random source</param>
    /// <returns>The randomly ordered enumerable</returns>
    public static IOrderedEnumerable<T> OrderByRandom<T>(this IEnumerable<T> enumerable, Random random)
        => enumerable.OrderBy(_ => random.Next());
}
