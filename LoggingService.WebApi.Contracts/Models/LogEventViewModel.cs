namespace LoggingService.WebApi.Contracts.Models;

public sealed class LogEventViewModel
{
    public required Guid Id { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string CollectionName { get; init; }
    public required LogEventLevelViewModel LogLevel { get; init; }
    public required string Message { get; init; }
    public required Dictionary<string, string> Args { get; init; }
}
