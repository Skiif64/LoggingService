using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.EventCollections.Commands.Queries;
internal sealed class GetPagedEventCollectionQueryHandler
    : IQueryHandler<GetPagedEventCollectionQuery, PagedList<EventCollectionDto>>
{
    private readonly IEventCollectionRepository _collectionRepository;
    private readonly ILogger<GetPagedEventCollectionQueryHandler> _logger;
    public GetPagedEventCollectionQueryHandler(IEventCollectionRepository collectionRepository, ILogger<GetPagedEventCollectionQueryHandler> logger)
    {
        _collectionRepository = collectionRepository;
        _logger = logger;
    }
    public async Task<Result<PagedList<EventCollectionDto>>> Handle(GetPagedEventCollectionQuery request, CancellationToken cancellationToken)
    {
        var collections = await _collectionRepository.GetPagedAsync(request.PageIndex, request.PageSize, cancellationToken);
        var mappedCollections = collections.Convert(collection => new EventCollectionDto
        {
            Id = collection.Id,
            Name = collection.Name,
        });
        return Result.Success(mappedCollections);
    }
}
