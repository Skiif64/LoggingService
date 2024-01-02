using LoggingService.Application.Base.Messaging;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
public record CreateLogEventCommand(Guid CollectionId, CreateLogEventDto Model) : ICommand
{
}
