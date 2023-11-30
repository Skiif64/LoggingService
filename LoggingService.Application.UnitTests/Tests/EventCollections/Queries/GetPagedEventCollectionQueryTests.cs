using AutoFixture;
using LoggingService.Application.Features.EventCollections.Commands.Queries;
using LoggingService.Application.UnitTests.Extensions;
using LoggingService.Domain.Features.EventCollections;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.UnitTests.Tests.EventCollections.Queries;
public class GetPagedEventCollectionQueryTests : TestBase
{
    private readonly Mock<IEventCollectionRepository> _collectionRepositoryMock;
    private readonly ILogger<GetPagedEventCollectionQueryHandler> _logger;
    private readonly GetPagedEventCollectionQueryHandler _sut;

    public GetPagedEventCollectionQueryTests()
    {
        _collectionRepositoryMock = new Mock<IEventCollectionRepository>();
        _logger = LoggerFactory.CreateLogger<GetPagedEventCollectionQueryHandler>();
        _sut = new GetPagedEventCollectionQueryHandler(_collectionRepositoryMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnResultWithItems()
    {
        var collections = Fixture.CreateMany<EventCollection>(10);
        _collectionRepositoryMock.Setup(x => x.GetPagedAsync(0, 20, CancellationToken))
            .ReturnsAsync(collections.ToPagedList(0, 20));
        var query = new GetPagedEventCollectionQuery(0, 20);

        var result = await _sut.Handle(query, CancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Items.Should().NotBeEmpty();
    }
}
