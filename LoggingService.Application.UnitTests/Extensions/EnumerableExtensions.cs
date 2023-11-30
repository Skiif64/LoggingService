using LoggingService.Domain.Shared;

namespace LoggingService.Application.UnitTests.Extensions;
public static class EnumerableExtensions
{
    public static PagedList<TItem> ToPagedList<TItem>(this IEnumerable<TItem> enumerable, int pageIndex, int pageSize)
        => PagedList.Create(enumerable, pageIndex, pageSize);
}
