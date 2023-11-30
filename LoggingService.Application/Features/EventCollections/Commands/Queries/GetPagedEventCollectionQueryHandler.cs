using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.Features.EventCollections.Commands.Queries;
internal sealed class GetPagedEventCollectionQueryHandler
    : IQueryHandler<GetPagedEventCollectionQuery, PagedList<EventCollection>>
{
    private readonly IEventCollectionRepository _collectionRepository;
    private readonly ILogger<GetPagedEventCollectionQueryHandler> _logger;
    public GetPagedEventCollectionQueryHandler(IEventCollectionRepository collectionRepository, ILogger<GetPagedEventCollectionQueryHandler> logger)
    {
        _collectionRepository = collectionRepository;
        _logger = logger;
    }
    public async Task<Result<PagedList<EventCollection>>> Handle(GetPagedEventCollectionQuery request, CancellationToken cancellationToken)
    {
        var collections = await _collectionRepository.GetPagedAsync(request.PageIndex, request.PageSize, cancellationToken);

        return Result.Success(collections);
    }
}
