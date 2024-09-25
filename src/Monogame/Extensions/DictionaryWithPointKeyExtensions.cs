namespace Tourmi.Monogame.Extensions;

/// <summary>
/// Extension methods for a <see cref="Dictionary{TKey, TValue}"/> that has a <see cref="Point"/> Key
/// </summary>
public static class DictionaryWithPointKeyExtensions
{
    /// <summary>
    /// Returns a subset of a <see cref="Dictionary{TKey, TValue}"/> with a <see cref="Point"/> Key. 
    /// <para/>The subset returned will have all the keys contained within the given <paramref name="subsetBounds"/>, 
    ///     including the Left and Top bounds, but excluding the Right and Bottom ones.
    /// <para/>Should a value not exist in the <paramref name="dictionary"/> for a given point, it will use <paramref name="defaultValue"/> instead.
    /// </summary>
    /// <typeparam name="T">Type of values in the dictionary</typeparam>
    /// <param name="dictionary">Dictionary to operate on</param>
    /// <param name="subsetBounds">The bounds to fetch the keys with</param>
    /// <param name="defaultValue">The default value, if a key does not exist</param>
    /// <returns>The new dictionary</returns>
    public static Dictionary<Point, T> Subset<T>(this Dictionary<Point, T> dictionary, Rectangle subsetBounds, T defaultValue)
    {
        _ = dictionary.ThrowIfNull();
        Dictionary<Point, T> newDict = [];

        for (var i = subsetBounds.Left; i < subsetBounds.Right; i++)
        {
            for (var j = subsetBounds.Top; j < subsetBounds.Bottom; j++)
            {
                newDict[new Point(i, j)] = dictionary.TryGetValue(new Point(i, j), out var value) ? value : defaultValue;
            }
        }

        return newDict;
    }
}
