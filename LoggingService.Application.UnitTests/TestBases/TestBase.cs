using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LoggingService.Application.UnitTests.TestBases;
public abstract class TestBase
{
    protected ILoggerFactory LoggerFactory { get; }
    protected CancellationToken CancellationToken { get; }

    public TestBase()
    {
        LoggerFactory = new NullLoggerFactory();
        CancellationToken = new CancellationToken();
    }
}
