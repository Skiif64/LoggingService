using LoggingService.Application.Base.Messaging;

namespace LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
public sealed record CreateLogEventBatchedCommand(Guid CollectionId, IEnumerable<CreateLogEventDto> Models)
    : ICommand
{
}