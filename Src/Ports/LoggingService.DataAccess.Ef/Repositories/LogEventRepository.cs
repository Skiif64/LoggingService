﻿using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;

namespace LoggingService.DataAccess.Ef.Repositories;
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

    public async Task InsertManyAsync(IEnumerable<LogEvent> entities, CancellationToken cancellationToken)
    {
        await Context.AddRangeAsync(entities, cancellationToken);
    }
}
