namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extensions operating on <see cref="IList{T}"/>
/// </summary>
public static class ListExtensions
{
    extension<T>(IList<T> list)
    {
        /// <summary>
        /// Pops the last element of the list and returns it.
        /// </summary>
        public T Pop()
        {
            _ = list.ThrowIfNullOrEmpty();

            var index = ^1;

            var item = list[index];
            list.RemoveAt(list.Count - 1);
            return item;
        }

        /// <summary>
        /// Removes the item at <paramref name="index"/>, swapping it with the last element of the list.
        /// </summary>
        public void RemoveSwap(Index index)
        {
            _ = index.ThrowIfOutOfBounds(list.ThrowIfNull().Count);

            list[index] = list[^1];
            _ = list.Pop();
        }
    }
}
