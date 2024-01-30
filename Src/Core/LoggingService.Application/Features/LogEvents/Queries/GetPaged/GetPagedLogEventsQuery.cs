using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;

namespace LoggingService.Application.Features.LogEvents.Queries.GetPaged;
public sealed record GetPagedLogEventsQuery(Guid CollectionId, int PageIndex, int PageSize)
    : IQuery<PagedList<LogEvent>>
{
}
