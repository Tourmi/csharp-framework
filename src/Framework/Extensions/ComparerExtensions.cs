namespace Tourmi.Framework.Extensions;

/// <summary>
/// Extension methods for comparers
/// </summary>
public static class ComparerExtensions
{
    extension(Comparer)
    {
        /// <summary>
        /// Returns a new comparer from the given func
        /// </summary>
        public static IComparer<T> FromFunc<T>(Func<T, T, int> compareFunc) => new FuncComparer<T>(compareFunc.ThrowIfNull());
    }

    private class FuncComparer<T>(Func<T, T, int> compareFunc) : Comparer<T>
    {
        private readonly Func<T, T, int> _compareFunc = compareFunc;

        public override int Compare(T? x, T? y)
        {
            if (x is null)
            {
                return y is null ? 0 : -1;
            }

            if (y is null)
            {
                return 1;
            }

            return _compareFunc(x, y);
        }
    }
}
