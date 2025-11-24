namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extension methods for <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/>
/// </summary>
public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> enumerable)
    {
        /// <summary>
        /// Orders the given randomly
        /// </summary>
        /// <param name="random">The random source</param>
        /// <returns>The randomly ordered enumerable</returns>
        public IOrderedEnumerable<T> OrderBy(Random random) => enumerable.OrderBy(_ => random.Next());
    }

    extension<T>(IEnumerable<T>? enumerable)
    {
        /// <summary>
        /// Returns an empty enumerable if the given enumerable is null.
        /// </summary>
        public IEnumerable<T> EmptyIfNull() => enumerable ?? [];
    }
}
