using LoggingService.Application.Base.Messaging;

namespace LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
public sealed record CreateLogEventBatchedCommand(string CollectionName, IEnumerable<CreateLogEventDto> Models)
    : ICommand
{
}