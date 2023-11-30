using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
public record CreateLogEventCommand(string CollectionName,
                                    LogEventLevel Level,
                                    DateTime Timestamp,
                                    Guid CollectionId,
                                    string Message,
                                    Dictionary<string, string> Args) : ICommand
{
}
