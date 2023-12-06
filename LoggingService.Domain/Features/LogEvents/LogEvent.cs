using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.LogEvents;
public class LogEvent : BaseEntity //TODO: private setters, ctor
{
    public DateTime Timestamp { get; set; }
    public Guid CollectionId { get; set; }
    public LogEventLevel LogLevel { get; set; }
    public string Message { get; set; } = null!;
    public Dictionary<string, string> Args { get; set; } = null!;
}
