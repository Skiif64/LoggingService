using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.WebApi.Contracts;

public sealed class CreateLogEventViewModel
{
    public required DateTime Timestamp { get; init; }
    public required LogEventLevel LogLevel { get; init; } //TODO: use viewmodel enum
    public required string Message { get; init; }
    public required Dictionary<string, string> Args { get; init; }
}
