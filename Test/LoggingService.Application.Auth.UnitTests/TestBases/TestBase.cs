using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace LoggingService.Application.Auth.UnitTests.TestBases;
public abstract class TestBase
{
    protected ILoggerFactory LoggerFactory { get; }
    protected CancellationToken CancellationToken { get; }
    protected IFixture Fixture { get; }

    public TestBase()
    {
        LoggerFactory = new NullLoggerFactory();
        CancellationToken = new CancellationToken();
        Fixture = new Fixture()
            .Customize(new AutoMoqCustomization());
    }
}
