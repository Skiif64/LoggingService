using LoggingService.Application.Base;
using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Errors;
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
    private readonly ILogger<CreateLogEventBatchedCommandHandler> _logger;
    public CreateLogEventBatchedCommandHandler(ILogEventRepository logRepository,
                                               IEventCollectionRepository collectionRepository,
                                               IUnitOfWork unitOfWork,
                                               ILogger<CreateLogEventBatchedCommandHandler> logger)
    {
        _logRepository = logRepository;
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
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
        var logs = request.Models.Select(log 
            => new LogEvent(Guid.NewGuid(), DateTime.UtcNow, log.Timestamp,
                    collection.Id, log.LogLevel, log.Message, log.Args));
        await _logRepository.InsertManyAsync(logs, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            _logger.LogCritical("Unable to save changes to database | exception: {message}",
                _unitOfWork.SaveChangesException.Message);
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        return Result.Success();
    }
}
