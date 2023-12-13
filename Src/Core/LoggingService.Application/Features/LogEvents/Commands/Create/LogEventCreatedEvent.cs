using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
public sealed record LogEventCreatedEvent(string CollectionName, IEnumerable<LogEvent> Logs) : IEvent //TODO: move
{
}
