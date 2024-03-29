﻿using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LoggingService.DataAccess.Ef.Repositories;
internal sealed class EventCollectionRepository : BaseRepository<EventCollection>, IEventCollectionRepository
{
    public EventCollectionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistByNameAsync(string name, CancellationToken cancellationToken)
        => await Context.EventCollections
                .AnyAsync(collection => collection.Name == name, cancellationToken);


    public async Task<EventCollection?> GetByNameAsync(string name, CancellationToken cancellationToken)
        => await Context.EventCollections
                .FirstOrDefaultAsync(collection => collection.Name == name, cancellationToken);

    public async Task<PagedList<EventCollection>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
        => await AsyncPagedList.CreateAsync(
            Context.EventCollections.OrderBy(collection => collection.Name),
            pageIndex, pageSize, cancellationToken);
}
