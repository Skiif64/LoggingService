using AutoFixture;
using LoggingService.Domain.Features.LogEvents;
using System.Text;

namespace LoggingService.Application.UnitTests.Fixtures;
public class LogEventCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var args = fixture.Create<Dictionary<string, string>>();
        var message = CreateMessage(fixture, args);
        fixture.Register<LogEvent>(() =>
        {
            return LogEvent.Create(
                fixture.Create<DateTime>(),
                fixture.Create<Guid>(),
                fixture.Create<LogEventLevel>(),
                message,
                args).Value!;
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
