using AutoFixture.AutoMoq;
using LoggingService.Application.Base;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace LoggingService.Application.Auth.UnitTests.TestBases;
public abstract class TestBase
{
    protected ILoggerFactory LoggerFactory { get; }
    protected Mock<IUnitOfWork> UnitOfWorkMock { get; }
    protected CancellationToken CancellationToken { get; }
    protected IFixture Fixture { get; }

    public TestBase()
    {
        LoggerFactory = new NullLoggerFactory();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        CancellationToken = new CancellationToken();
        Fixture = new Fixture()
            .Customize(new AutoMoqCustomization());
    }
}
