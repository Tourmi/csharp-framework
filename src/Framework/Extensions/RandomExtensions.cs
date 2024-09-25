namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extension methods for <see cref="Random"/>
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Returns a random element from the given <paramref name="collection"/>
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <param name="random">Random source</param>
    /// <param name="collection">The collection to grab an item from</param>
    /// <returns>The selected item</returns>
    public static T SingleFrom<T>(this Random random, ICollection<T> collection)
        => collection.ThrowIfNullOrEmpty().ElementAt(random.ThrowIfNull().Next(collection.Count));

    /// <summary>
    /// Picks <paramref name="amount"/> random elements from the given <paramref name="collection"/>
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <param name="random">Random source, called up to N times, where N is the amount of items in <paramref name="collection"/></param>
    /// <param name="collection">The collection to take from</param>
    /// <param name="amount">The amount to take from the collection</param>
    /// <returns>The elements taken from the collection</returns>
    public static IEnumerable<T> MultipleFrom<T>(this Random random, ICollection<T> collection, int amount)
    {
        _ = random.ThrowIfNull();
        _ = amount.ThrowIfNegative();
        _ = collection.ThrowIfContainsLessThan(amount);

        for (var i = 0; i < collection.Count && amount > 0; i++)
        {
            if (random.NextDouble() <= amount / (double)(collection.Count - i))
            {
                amount--;
                yield return collection.ElementAt(i);
            }
        }
    }
}
