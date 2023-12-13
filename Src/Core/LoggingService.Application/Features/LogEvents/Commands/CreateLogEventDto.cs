using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Application.Features.LogEvents.Commands;
public sealed record CreateLogEventDto(LogEventLevel LogLevel,
                                       DateTime Timestamp,
                                       string Message,
                                       Dictionary<string, string> Args)
{
}
