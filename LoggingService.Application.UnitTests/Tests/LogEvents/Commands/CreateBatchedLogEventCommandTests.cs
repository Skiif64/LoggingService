using AutoFixture;
using LoggingService.Application.Errors;
using LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
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
        var command = Fixture.Create<CreateLogEventBatchedCommand>();
        var collection = Fixture.Build<EventCollection>()
            .With(prop => prop.Name, command.CollectionName)
            .Create();
        _collectionRepositoryMock.Setup(x => x.GetByNameAsync(command.CollectionName, CancellationToken))
            .ReturnsAsync(collection);

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCollectionDoesNotExists()
    {
        var command = Fixture.Create<CreateLogEventBatchedCommand>();

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(EventCollectionErrors.NotFound(nameof(EventCollection.Name), command.CollectionName));
    }

    [Fact]
    public async Task Handle_ShouldReturnSaveChangesError_WhenCannotSaveChanges()
    {
        var command = Fixture.Create<CreateLogEventBatchedCommand>();
        var collection = Fixture.Build<EventCollection>()
            .With(prop => prop.Name, command.CollectionName)
            .Create();
        _collectionRepositoryMock.Setup(x => x.GetByNameAsync(command.CollectionName, CancellationToken))
            .ReturnsAsync(collection);
        UnitOfWorkMock.SetupGet(prop => prop.SaveChangesException)
            .Returns(new Exception());

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ApplicationErrors.SaveChangesError);
    }
}
