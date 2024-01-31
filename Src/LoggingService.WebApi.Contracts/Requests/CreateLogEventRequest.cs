using LoggingService.WebApi.Contracts.Models;

namespace LoggingService.WebApi.Contracts.Requests;

public sealed class CreateLogEventRequest
{
    public required Guid CollectionId { get; init; }
    public required CreateLogEventViewModel LogModel { get; init; }
}