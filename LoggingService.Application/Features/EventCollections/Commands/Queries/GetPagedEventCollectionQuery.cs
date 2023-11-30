using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Shared;

namespace LoggingService.Application.Features.EventCollections.Commands.Queries;
public sealed record GetPagedEventCollectionQuery(int PageIndex, int PageSize)
    : IQuery<PagedList<EventCollection>>
{
}
