using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
public record CreateLogEventCommand(LogEventLevel Level,
                                    DateTime Timestamp,
                                    string Message,
                                    Dictionary<string, string> Args) : ICommand
{
}
