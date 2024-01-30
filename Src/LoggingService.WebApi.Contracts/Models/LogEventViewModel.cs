namespace LoggingService.WebApi.Contracts.Models;

public sealed class LogEventViewModel
{
    public required DateTime Timestamp { get; init; }
    public required Guid CollectionId { get; init; }
    public required LogEventLevelViewModel LogLevel { get; init; }
    public required string MessageTemplate { get; init; }
    public required Dictionary<string, string> Properties { get; init; }
}
