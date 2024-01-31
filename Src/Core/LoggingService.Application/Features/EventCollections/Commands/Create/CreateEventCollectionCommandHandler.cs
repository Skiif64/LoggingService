using LoggingService.Application.Base;
using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.EventCollections.Commands.Create;
internal sealed class CreateEventCollectionCommandHandler
    : ICommandHandler<CreateEventCollectionCommand>
{
    private readonly IEventCollectionRepository _collectionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateEventCollectionCommandHandler> _logger;

    public CreateEventCollectionCommandHandler(IEventCollectionRepository collectionRepository,
                                               IUnitOfWork unitOfWork,
                                               ILogger<CreateEventCollectionCommandHandler> logger)
    {
        _collectionRepository = collectionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result> Handle(CreateEventCollectionCommand request, CancellationToken cancellationToken)
    {
        if(await _collectionRepository.ExistByNameAsync(request.Name, cancellationToken))
        {
            _logger.LogWarning("EventCollection with Name = {name} already exists", request.Name);
            return Result.Failure(
                EventCollectionErrors.Duplicate(nameof(EventCollection.Name), request.Name));
        }

        var collection = new EventCollection(request.Name, request.ApplicationId);

        await _collectionRepository.InsertAsync(collection, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
