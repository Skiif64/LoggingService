using AutoFixture;
using LoggingService.Application.Features.LogEvents.Commands;
using LoggingService.Domain.Features.LogEvents;
using System.Text;

namespace LoggingService.Application.UnitTests.Fixtures;
public class CreateLogEventDtoCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<CreateLogEventDto>(() =>
        {
            var args = fixture.Create<Dictionary<string, string>>();
            var message = CreateMessage(fixture, args);
            return new CreateLogEventDto(
                fixture.Create<LogEventLevel>(),
                fixture.Create<DateTime>(),
                message,
                args);
        });
    }

    private string CreateMessage(IFixture fixture, Dictionary<string, string> args)
    {
        var sb = new StringBuilder(fixture.Create<string>());
        foreach(var arg in args)
        {
            sb.Insert(sb.Length, $" {{{arg.Key}}} ");
        }

        return sb.ToString();
    }
}
