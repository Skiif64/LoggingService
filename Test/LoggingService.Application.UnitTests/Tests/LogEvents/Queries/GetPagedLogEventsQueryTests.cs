using AutoFixture;
using LoggingService.Application.Features.LogEvents.Queries.GetPaged;
using LoggingService.Application.UnitTests.Extensions;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Tests.Shared.Extensions;
using LoggingService.Tests.Shared.Specimen;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.UnitTests.Tests.LogEvents.Queries;
public class GetPagedLogEventsQueryTests : TestBase
{
    private readonly Mock<ILogEventRepository> _logRepository;
    private readonly Mock<IEventCollectionRepository> _collectionRepository;
    private readonly ILogger<GetPagedLogEventsQueryHandler> _logger;
    private readonly GetPagedLogEventsQueryHandler _sut;

    public GetPagedLogEventsQueryTests()
    {
        _logger = LoggerFactory.CreateLogger<GetPagedLogEventsQueryHandler>();
        _logRepository = new Mock<ILogEventRepository>();
        _collectionRepository = new Mock<IEventCollectionRepository>();
        _sut = new GetPagedLogEventsQueryHandler(_logRepository.Object, _collectionRepository.Object, _logger);
    }

    [Fact]       
    public async Task Handle_ShouldReturnSuccessResult()
    {
        var query = new GetPagedLogEventsQuery(Guid.NewGuid(), 0, 20);
        var collection = Fixture.Create<EventCollection>();
        Fixture.Customizations.Add(new LogEventSpecimenBuilder(collection.Id));
        var logs = Fixture.CreateMany<LogEvent>(20);
        _collectionRepository.Setup(x => x.GetByIdAsync(query.CollectionId, default))
            .ReturnsAsync(collection);
        _logRepository.Setup(x => x.GetPagedByCollectionIdAsync(collection.Id, query.PageIndex, query.PageSize, CancellationToken))
            .ReturnsAsync(logs.ToPagedList(0, 20));

        var result = await _sut.Handle(query, CancellationToken);

        Assert.True(result.IsSuccess, $"Result is not success | error: {result.Error}");
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCollectionNotFound()
    {       
        var query = new GetPagedLogEventsQuery(Guid.NewGuid(), 0, 20);
       
        var result = await _sut.Handle(query, CancellationToken);

        Assert.False(result.IsSuccess, "Result is success");
        Assert.Equal(EventCollectionErrors.NotFound(nameof(EventCollection.Name), query.CollectionId), result.Error);
    }
}
