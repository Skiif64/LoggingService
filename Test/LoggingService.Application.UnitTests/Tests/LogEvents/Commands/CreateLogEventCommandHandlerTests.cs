using AutoFixture;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.UnitTests.Tests.LogEvents.Commands;
public class CreateLogEventCommandHandlerTests : TestBase
{
    private readonly Mock<ILogEventRepository> _logRepositoryMock;
    private readonly Mock<IEventCollectionRepository> _collectionRepositoryMock;
    
    private readonly ILogger<CreateLogEventCommandHandler> _logger;
    private readonly CreateLogEventCommandHandler _sut;

    public CreateLogEventCommandHandlerTests()
    {
        _logRepositoryMock = new Mock<ILogEventRepository>();
        _collectionRepositoryMock = new Mock<IEventCollectionRepository>();
        _logger = LoggerFactory.CreateLogger<CreateLogEventCommandHandler>();
        _sut = new CreateLogEventCommandHandler(
            _logRepositoryMock.Object, _collectionRepositoryMock.Object,
            UnitOfWorkMock.Object, EventBusMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenDataIsValid()
    {
        var command = Fixture.Create<CreateLogEventCommand>();
        var collection = Fixture.Build<EventCollection>()
            .Create();
        _collectionRepositoryMock.Setup(x => x.GetByIdAsync(command.CollectionId, default))
            .ReturnsAsync(collection);

        var result = await _sut.Handle(command, CancellationToken);

        Assert.True(result.IsSuccess, $"Result is not success | error: {result.Error}");
    }

    [Fact]
    public async Task Handle_ShouldReturnCollectionNotFoundError_WhenEventCollectionNotFound()
    {
        var command = Fixture.Create<CreateLogEventCommand>();

        var result = await _sut.Handle(command, CancellationToken);

        Assert.False(result.IsSuccess);
        Assert.Equal(EventCollectionErrors.NotFound(nameof(EventCollection.Name), command.CollectionId), result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnParseError_WhenMessageDoesNotMatchToArgs()
    {
        var command = Fixture.Create<CreateLogEventCommand>();
        command.Model.Args.Add("NewArg", "arg");
        _collectionRepositoryMock.Setup(x => x.GetByIdAsync(command.CollectionId, default))
           .ReturnsAsync(Fixture.Create<EventCollection>());

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(LogEventsErrors.ParseError);
    }
}
