using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using AutoFixture;
using AutoFixture.AutoMoq;
using LoggingService.Tests.Shared.FixtureCustomizations;

namespace LoggingService.Application.UnitTests.TestBases;
public abstract class TestBase
{
    protected ILoggerFactory LoggerFactory { get; }
    protected CancellationToken CancellationToken { get; }
    protected Mock<IUnitOfWork> UnitOfWorkMock { get; }
    protected Mock<IEventBus> EventBusMock { get; }
    protected IFixture Fixture { get; }

    public TestBase()
    {
        LoggerFactory = new NullLoggerFactory();
        CancellationToken = new CancellationToken();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        EventBusMock = new Mock<IEventBus>();
        Fixture = new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new CreateLogEventDtoCustomization());
    }
}
