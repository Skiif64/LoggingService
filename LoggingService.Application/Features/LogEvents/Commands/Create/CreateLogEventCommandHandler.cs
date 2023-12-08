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
        var collection = await _eventCollectionRepository.GetByNameAsync(request.CollectionName, cancellationToken);
        if(collection is null)
        {
            _logger.LogWarning("EventCollection with name: {name} was not found", request.CollectionName);
            return Result.Failure(EventCollectionErrors.NotFound(nameof(collection.Name), request.CollectionName));
        }
        //TODO: refactor log event
        var eventLog = new LogEvent
        {
            Id = Guid.NewGuid(),
            CreatedAtUtc = DateTime.UtcNow,
            Timestamp = request.Model.Timestamp,
            CollectionId = collection.Id,
            LogLevel = request.Model.LogLevel,
            Message = request.Model.Message,
            Args = request.Model.Args
        };

        await _logRepository.InsertAsync(eventLog, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            _logger.LogCritical("Unable to save changes to database | exception: {message}", _unitOfWork.SaveChangesException);
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        await _bus.PublishAsync(new LogEventCreatedEvent(new[] { eventLog }), cancellationToken);

        return Result.Success();
    }
}
