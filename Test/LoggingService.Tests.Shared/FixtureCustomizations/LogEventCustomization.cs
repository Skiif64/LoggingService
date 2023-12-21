using AutoFixture;
using LoggingService.Domain.Features.LogEvents;
using System.Text;

namespace LoggingService.Tests.Shared.FixtureCustomizations;
public class LogEventCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var args = fixture.Create<Dictionary<string, string>>();
        var message = CreateMessage(fixture, args);
        fixture.Register<LogEvent>(() =>
        {
            var logEvent = LogEvent.Create(
                fixture.Create<DateTime>(),
                fixture.Create<Guid>(),
                fixture.Create<LogEventLevel>(),
                message,
                args);
            if(logEvent.IsSuccess)
            {
                return logEvent.Value!;
            }
            else
            {
                throw new Exception();
            }
        });
    }

    private string CreateMessage(IFixture fixture, Dictionary<string, string> args)
    {
        var sb = new StringBuilder(fixture.Create<string>());
        foreach (var arg in args)
        {
            sb.Insert(sb.Length, $" {{{arg.Key}}} ");
        }

        return sb.ToString();
    }
}