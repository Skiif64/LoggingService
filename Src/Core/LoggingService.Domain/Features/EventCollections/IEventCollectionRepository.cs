﻿using LoggingService.Domain.Base;
using LoggingService.Domain.Shared;

namespace LoggingService.Domain.Features.EventCollections;
public interface IEventCollectionRepository : ICrudRepository<EventCollection>
{
    Task<bool> ExistByNameAsync(string name, CancellationToken cancellationToken);
    Task<PagedList<EventCollection>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
}
