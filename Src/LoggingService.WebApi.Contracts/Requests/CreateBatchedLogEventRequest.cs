using LoggingService.WebApi.Contracts.Models;

namespace LoggingService.WebApi.Contracts.Requests;

public sealed class CreateBatchedLogEventRequest
{
    public required Guid CollectionId { get; init; }
    public required IEnumerable<CreateLogEventViewModel> LogModels { get; init; }
}