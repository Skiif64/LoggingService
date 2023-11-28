using AutoFixture;
using Castle.Core.Logging;
using LoggingService.Application.Base;
using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Errors;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Application.UnitTests.TestBases;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.Extensions.Logging;

namespace LoggingService.Application.UnitTests.Tests.LogEvents.Commands;
public class CreateLogEventCommandHandlerTests : TestBase
{
    private readonly Mock<ILogEventRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEventBus> _eventBusMock;
    private readonly ILogger<CreateLogEventCommandHandler> _logger;
    private readonly CreateLogEventCommandHandler _sut;

    public CreateLogEventCommandHandlerTests()
    {
        _repositoryMock = new Mock<ILogEventRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _eventBusMock = new Mock<IEventBus>();
        _logger = LoggerFactory.CreateLogger<CreateLogEventCommandHandler>();
        _sut = new CreateLogEventCommandHandler(
            _repositoryMock.Object, _unitOfWorkMock.Object, _eventBusMock.Object, _logger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenDataIsValid()
    {
        var command = new Fixture().Create<CreateLogEventCommand>();        

        var result = await _sut.Handle(command, CancellationToken);

        Assert.True(result.IsSuccess, $"Result is not success | error: {result.Error}");
    }

    [Fact]
    public async Task Handle_ShouldReturnDatabaseError_WhenUnitOfWorkCannotSaveChanges()
    {
        _unitOfWorkMock.Setup(x => x.SaveChangesException)
            .Returns(new Exception());
        var command = new Fixture().Create<CreateLogEventCommand>();

        var result = await _sut.Handle(command, CancellationToken);

        Assert.False(result.IsSuccess, $"Result is success");
        Assert.Equal(ApplicationErrors.SaveChangesError, result.Error);
    }
}
