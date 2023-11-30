namespace LoggingService.Domain.Shared;
public class PagedList<TItem>
{
    public IReadOnlyCollection<TItem> Items { get; }
    public int CurrentCount { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasNextPage { get; }
    public bool HasPrevPage { get; }

    private PagedList(IReadOnlyCollection<TItem> items,
                      int pageIndex,
                      int pageSize,
                      int totalPages,
                      int totalCount)
    {
        Items = items;
        CurrentCount = items.Count;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalCount = totalCount;
        HasNextPage = pageIndex < totalPages;
        HasPrevPage = pageIndex != 0;
    }

    public static PagedList<TItem> Create(IQueryable<TItem> queryable, int pageIndex, int pageSize)
    {
        var totalCount = queryable.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var items = queryable
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToList();

        return new PagedList<TItem>(items, pageIndex, pageSize, totalPages, totalCount);
    }

    public PagedList<TTo> Convert<TTo>(IEnumerable<TTo> enumerable)
    {
        if (enumerable.Count() != CurrentCount)
        {
            throw new ArgumentException("Enumerable count did not match pagedList count", nameof(enumerable));
        }

        return new PagedList<TTo>(enumerable.ToList(), PageIndex, PageSize, TotalPages, TotalCount);
    }
}

public static class PagedList
{
    public static PagedList<TItem> Create<TItem>(IQueryable<TItem> queryable, int pageIndex, int pageSize)
        => PagedList<TItem>.Create(queryable, pageIndex, pageSize);
    public static PagedList<TItem> Create<TItem>(IEnumerable<TItem> enumerable, int pageIndex, int pageSize)
        => PagedList<TItem>.Create(enumerable.AsQueryable(), pageIndex, pageSize);
}
