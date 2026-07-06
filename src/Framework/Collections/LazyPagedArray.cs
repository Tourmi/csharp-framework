namespace Tourmi.Framework.Collections;

/// <summary>
/// A paged array which instantiates full pages of data at once,
/// initialized at their default values.
/// Can be indexed by and inserted into at any locations.
/// Does not keep track of whether or not an index has been inserted into.
/// </summary>
public sealed class LazyPagedArray<T>
{
    private const byte MaximumPageSize = 32;

    private readonly ulong _pageSize;
    private readonly uint _pageIdMask;
    private readonly uint _pageIndexMask;

    private readonly Dictionary<PageId, T?[]> _pages = [];

    /// <summary>
    /// Generates a paged array with a page size of 2^<paramref name="pageSizePowerOf2"/>.
    /// </summary>
    /// <param name="pageSizePowerOf2">
    ///     The power of two to use for each page of the paged array. 
    ///     Cannot be greater than 32, since the array is indexed with a <see cref="uint"/>.
    /// </param>
    public LazyPagedArray(byte pageSizePowerOf2 = 12)
    {
        _ = pageSizePowerOf2.ThrowIfGreaterThan(MaximumPageSize);

        _pageSize = 1ul << pageSizePowerOf2;
        _pageIndexMask = unchecked((uint)(_pageSize - 1));
        _pageIdMask = ~_pageIndexMask;
    }

    /// <summary>
    /// Returns a reference to the index in the paged array, initializing pages if needed.
    /// </summary>
    public ref T? this[uint index] => ref GetPage(index)[ToPageIndex(index).Index];

    /// <summary>
    /// Inserts <paramref name="value"/> at the given <paramref name="index"/> in the paged array.
    /// </summary>
    public void Insert(uint index, T value) => GetPage(index)[ToPageIndex(index).Index] = value;

    /// <summary>
    /// Ensures that the paged array is initialized for the given item count, starting at the given index.
    /// </summary>
    public void EnsureCapacity(uint count, uint startIndex = 0)
    {
        ulong currentIndex = startIndex & _pageIdMask;
        var targetIndex = Math.Min(startIndex + (ulong)count, uint.MaxValue);
        while (currentIndex <= targetIndex)
        {
            _ = GetPage(checked((uint)currentIndex));
            currentIndex += _pageSize;
        }
    }

    private T?[] GetPage(uint index)
    {
        var pageId = ToPageId(index);
        if (!_pages.TryGetValue(pageId, out var page))
        {
            page = new T[_pageSize];
            _pages.Add(pageId, page);
        }

        return page;
    }

    private PageId ToPageId(uint index) => new(index & _pageIdMask);
    private PageIndex ToPageIndex(uint index) => new(index & _pageIndexMask);

    private readonly record struct PageId(uint Id);
    private readonly record struct PageIndex(uint Index);
}
