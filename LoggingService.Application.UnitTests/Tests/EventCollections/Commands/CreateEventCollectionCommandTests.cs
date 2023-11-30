using Castle.Core.Logging;
using FluentAssertions;
using LoggingService.Application.Errors;
using LoggingService.Application.Features.EventCollections.Commands.Create;
using LoggingService.Domain.Features.EventCollections;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.UnitTests.Tests.EventCollections.Commands;
public class CreateEventCollectionCommandTests : TestBase
{
    private readonly Mock<IEventCollectionRepository> _collectionRepositoryMock;
    private readonly ILogger<CreateEventCollectionCommandHandler> _logger;
    private readonly CreateEventCollectionCommandHandler _sut;

    public CreateEventCollectionCommandTests()
    {
        _collectionRepositoryMock = new Mock<IEventCollectionRepository>();
        _logger = LoggerFactory.CreateLogger<CreateEventCollectionCommandHandler>();
        _sut = new CreateEventCollectionCommandHandler(_collectionRepositoryMock.Object, UnitOfWorkMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenCollectionIsValid()
    {
        var command = new CreateEventCollectionCommand("TestCollection");

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeTrue();

        _collectionRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<EventCollection>(), default), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnDuplicateError_WhenCollectionWithNameExists()
    {
        var command = new CreateEventCollectionCommand("TestCollection");
        _collectionRepositoryMock.Setup(x => x.ExistByNameAsync(command.Name, CancellationToken))
            .ReturnsAsync(true);

        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(EventCollectionErrors.Duplicate(nameof(EventCollection.Name), command.Name));

        _collectionRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<EventCollection>(), default), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnSaveChangesError_WhenUnableToSaveChanges()
    {
        var command = new CreateEventCollectionCommand("TestCollection");
        UnitOfWorkMock.SetupGet(x => x.SaveChangesException)
            .Returns(new Exception());
        var result = await _sut.Handle(command, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ApplicationErrors.SaveChangesError);
    }
}
