using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.WebApi.Contracts;

public sealed class LogEventViewModel
{
    public required Guid Id { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string CollectionName { get; init; }
    public required LogEventLevel LogLevel { get; init; }
    public required string Message { get; init; }
    public required Dictionary<string, string> Args { get; init; }
}
