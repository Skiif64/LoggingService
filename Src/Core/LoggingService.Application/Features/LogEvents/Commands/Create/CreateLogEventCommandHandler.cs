using LoggingService.Application.Base;
using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Errors;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
internal sealed class CreateLogEventCommandHandler
    : ICommandHandler<CreateLogEventCommand>
{
    private readonly ILogEventRepository _logRepository;
    private readonly IEventCollectionRepository _eventCollectionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _bus;
    private readonly ILogger<CreateLogEventCommandHandler> _logger;

    public CreateLogEventCommandHandler(ILogEventRepository logRepository,
                                        IEventCollectionRepository eventCollectionRepository,
                                        IUnitOfWork unitOfWork,
                                        IEventBus bus,
                                        ILogger<CreateLogEventCommandHandler> logger)
    {
        _logRepository = logRepository;
        _eventCollectionRepository = eventCollectionRepository;
        _unitOfWork = unitOfWork;
        _bus = bus;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateLogEventCommand request, CancellationToken cancellationToken)
    {        
        var collection = await _eventCollectionRepository.GetByIdAsync(request.CollectionId, cancellationToken);
        if(collection is null)
        {
            _logger.LogWarning("EventCollection with id: {name} was not found", request.CollectionId);
            return Result.Failure(EventCollectionErrors.NotFound(nameof(collection.Name), request.CollectionId));
        }
        //TODO: refactor log event
        var createResult = LogEvent.Create(
            request.Model.Timestamp, collection.Id, request.Model.LogLevel, request.Model.Message, request.Model.Args);
        if(!createResult.IsSuccess)
        {
            return createResult;
        }
        var eventLog = createResult.Value!;

        await _logRepository.InsertAsync(eventLog, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            _logger.LogCritical("Unable to save changes to database | exception: {message}", _unitOfWork.SaveChangesException);
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        await _bus.PublishAsync(new LogEventCreatedEvent(collection.Id, new[] { eventLog }), cancellationToken);

        return Result.Success();
    }
}
