using LoggingService.Application.Base;
using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Errors;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
internal sealed class CreateLogEventBatchedCommandHandler
    : ICommandHandler<CreateLogEventBatchedCommand>
{
    private readonly ILogEventRepository _logRepository;
    private readonly IEventCollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _bus;
    private readonly ILogger<CreateLogEventBatchedCommandHandler> _logger;
    public CreateLogEventBatchedCommandHandler(ILogEventRepository logRepository,
                                               IEventCollectionRepository collectionRepository,
                                               IUnitOfWork unitOfWork,
                                               IEventBus bus,
                                               ILogger<CreateLogEventBatchedCommandHandler> logger)
    {
        _logRepository = logRepository;
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
        _bus = bus;
        _logger = logger;
    }
    public async Task<Result> Handle(CreateLogEventBatchedCommand request, CancellationToken cancellationToken)
    {
        var collection = await _collectionRepository.GetByNameAsync(request.CollectionName, cancellationToken);
        if(collection is null)
        {
            return Result.Failure(
                EventCollectionErrors.NotFound(nameof(EventCollection.Name), request.CollectionName));
        }
        var logs = new List<LogEvent>();
        foreach(var logModel in request.Models)
        {
            var createResult = LogEvent.Create(
            logModel.Timestamp, collection.Id, logModel.LogLevel, logModel.Message, logModel.Args);
            if (!createResult.IsSuccess)
            {
                return createResult;
            }
            var log = createResult.Value!;
            logs.Add(log);
        }

        await _logRepository.InsertManyAsync(logs, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            _logger.LogCritical("Unable to save changes to database | exception: {message}",
                _unitOfWork.SaveChangesException.Message);
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        await _bus.PublishAsync(new LogEventCreatedEvent(collection.Name, logs), cancellationToken);
        return Result.Success();
    }
}
