using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.LogEvents.Queries.GetPaged;
internal sealed class GetPagedLogEventsQueryHandler
    : IQueryHandler<GetPagedLogEventsQuery, PagedList<LogEvent>>
{
    private readonly ILogEventRepository _logRepository;
    private readonly IEventCollectionRepository _collectionRepository;
    private readonly ILogger<GetPagedLogEventsQueryHandler> _logger;

    public GetPagedLogEventsQueryHandler(ILogEventRepository logRepository,
                                         IEventCollectionRepository collectionRepository,
                                         ILogger<GetPagedLogEventsQueryHandler> logger)
    {
        _logRepository = logRepository;
        _collectionRepository = collectionRepository;
        _logger = logger;
    }
    public async Task<Result<PagedList<LogEvent>>> Handle(GetPagedLogEventsQuery request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.GetByIdAsync(request.CollectionId, cancellationToken);
        if(collection is null)
        {
            _logger.LogWarning("EventCollection with name: {name} was not found", request.CollectionId);
            return Result.Failure<PagedList<LogEvent>>(
                EventCollectionErrors.NotFound(nameof(collection.Name), request.CollectionId));
        }

        var eventList = await _logRepository
            .GetPagedByCollectionIdAsync(collection.Id, request.PageIndex, request.PageSize, cancellationToken);
        return Result.Success(eventList);
    }
}
