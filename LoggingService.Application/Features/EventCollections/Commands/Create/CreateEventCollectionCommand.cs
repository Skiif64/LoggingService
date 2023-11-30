using LoggingService.Application.Base.Messaging;

namespace LoggingService.Application.Features.EventCollections.Commands.Create;
public sealed record CreateEventCollectionCommand(string Name)
    : ICommand
{
}
