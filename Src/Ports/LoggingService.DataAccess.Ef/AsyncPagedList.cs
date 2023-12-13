using LoggingService.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LoggingService.DataAccess.Ef;
internal sealed class AsyncPagedList<TEntity> : PagedList<TEntity>
{
    public AsyncPagedList(IReadOnlyCollection<TEntity> items,
                          int pageIndex,
                          int pageSize,
                          int totalPages,
                          int totalCount)
        : base(items, pageIndex, pageSize, totalPages, totalCount)
    {
    }
}

internal static class AsyncPagedList
{
    public static async Task<AsyncPagedList<TEntity>> CreateAsync<TEntity>(IQueryable<TEntity> queryable,
                                                                     int pageIndex,
                                                                     int pageSize,
                                                                     CancellationToken cancellationToken = default)
    {
        var totalCount = queryable.Count();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var items = await queryable
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new AsyncPagedList<TEntity>(items, pageIndex, pageSize, totalPages, totalCount);
    }
}
