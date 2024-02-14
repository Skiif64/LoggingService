using AutoFixture;
using LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Tests.Shared.Fakes;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.UnitTests.Tests.LogEvents.Commands;
public class CreateBatchedLogEventCommandTests : TestBase
{
    private readonly Mock<ILogEventRepository> _logRepositoryMock;
    private readonly Mock<IEventCollectionRepository> _collectionRepositoryMock;

    private readonly ILogger<CreateLogEventBatchedCommandHandler> _logger;
    private readonly CreateLogEventBatchedCommandHandler _sut;

    public CreateBatchedLogEventCommandTests()
    {
        _logRepositoryMock = new Mock<ILogEventRepository>();
        _collectionRepositoryMock = new Mock<IEventCollectionRepository>();
        _logger = LoggerFactory.CreateLogger<CreateLogEventBatchedCommandHandler>();
        _sut = new CreateLogEventBatchedCommandHandler(
            _logRepositoryMock.Object, _collectionRepositoryMock.Object,
            UnitOfWorkMock.Object, EventBusMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult()
    {
        var createDtos = Fake.LogEvent.CreateLogEventCreateDtoFaker().Generate(10);
        var collection = Fake.EventCollection.CreateEventCollectionFaker().Generate();
        var command = new CreateLogEventBatchedCommand(collection.Id, createDtos);
        _collectionRepositoryMock.Setup(x => x.GetByIdAsync(command.CollectionId, CancellationToken))
            .ReturnsAsync(collection);

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCollectionDoesNotExists()
    {
        var createDtos = Fake.LogEvent.CreateLogEventCreateDtoFaker().Generate(10);
        var command = new CreateLogEventBatchedCommand(Guid.NewGuid(), createDtos);

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(EventCollectionErrors.NotFound(nameof(EventCollection.Name), command.CollectionId));
    }

    [Fact]
    public async Task Handle_ShouldReturnParseError_WhenMessageDoesNotMatchToArgs()
    {
        var createDtos = Fake.LogEvent.CreateLogEventCreateDtoFaker().Generate(10);
        var collection = Fake.EventCollection.CreateEventCollectionFaker().Generate();
        var command = new CreateLogEventBatchedCommand(collection.Id, createDtos);
        createDtos[0].Args.Add("Invalid arg", "Invalid args");
        _collectionRepositoryMock.Setup(x => x.GetByIdAsync(command.CollectionId, CancellationToken))
           .ReturnsAsync(collection);

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(LogEventsErrors.ParseError);
    }
}
