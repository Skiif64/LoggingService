using LoggingService.Application.Base;
using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Errors;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
internal sealed class CreateLogEventCommandHandler
    : ICommandHandler<CreateLogEventCommand>
{
    private readonly ILogEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _bus;
    private readonly ILogger<CreateLogEventCommandHandler> _logger;

    public CreateLogEventCommandHandler(ILogEventRepository repository,
                                        IUnitOfWork unitOfWork,
                                        IEventBus bus,
                                        ILogger<CreateLogEventCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _bus = bus;
        _logger = logger;
    }

    public async Task<Result> Handle(CreateLogEventCommand request, CancellationToken cancellationToken)
    {
        //TODO: validate message template count and args count & names
        var eventLog = new LogEvent(
            Guid.NewGuid(), DateTime.UtcNow, request.Timestamp, request.Level, request.Message, request.Args);

        await _repository.CreateAsync(eventLog, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            _logger.LogCritical("Unable to save changes to database | exception: {message}", _unitOfWork.SaveChangesException);
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        await _bus.PublishAsync(new LogEventCreatedEvent(eventLog), cancellationToken);

        return Result.Success();
    }
}
