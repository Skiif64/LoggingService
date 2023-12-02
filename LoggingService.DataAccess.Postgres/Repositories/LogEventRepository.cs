using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;

namespace LoggingService.DataAccess.Postgres.Repositories;
internal sealed class LogEventRepository : BaseRepository<LogEvent>, ILogEventRepository
{

    public LogEventRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<PagedList<LogEvent>> GetPagedByCollectionIdAsync(Guid collectionId, int pageIndex, int pageSize, CancellationToken cancellationToken)
    {
        var logs = Context.LogEvents
            .Where(log => log.CollectionId == collectionId)
            .OrderByDescending(log => log.Timestamp);

        return await AsyncPagedList.CreateAsync(logs, pageIndex, pageSize, cancellationToken);
    }
}
