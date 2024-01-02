using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.LogEvents.Queries.GetPaged;
internal sealed class GetPagedLogEventsQueryHandler
    : IQueryHandler<GetPagedLogEventsQuery, PagedList<LogEventDto>>
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
    public async Task<Result<PagedList<LogEventDto>>> Handle(GetPagedLogEventsQuery request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.GetByIdAsync(request.CollectionId, cancellationToken);
        if(collection is null)
        {
            _logger.LogWarning("EventCollection with name: {name} was not found", request.CollectionId);
            return Result.Failure<PagedList<LogEventDto>>(
                EventCollectionErrors.NotFound(nameof(collection.Name), request.CollectionId));
        }

        var eventList = await _logRepository
            .GetPagedByCollectionIdAsync(collection.Id, request.PageIndex, request.PageSize, cancellationToken);
        var mappedList = eventList.Convert(log => new LogEventDto //TODO: use mapper
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            Args = log.Args,
            CollectionName = collection.Name,
            LogLevel = log.LogLevel,
            Message = log.Message,
        });
        return Result.Success(mappedList);
    }
}
