using LoggingService.Application.Base.Messaging;

namespace LoggingService.Application.Features.LogEvents.Commands.Create;
public record CreateLogEventCommand(string CollectionName, CreateLogEventDto Model) : ICommand
{
}
