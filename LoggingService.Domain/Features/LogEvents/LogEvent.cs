using LoggingService.Domain.Base;
using LoggingService.Domain.Features.EventCollections;

namespace LoggingService.Domain.Features.LogEvents;
public class LogEvent : BaseEntity //TODO: private setters, ctor
{
    public DateTime Timestamp { get; set; }
    public Guid CollectionId { get; set; }
    public EventCollection Collection { get; set; } = null!;
    public LogEventLevel LogLevel { get; set; }
    public string Message { get; set; } = null!;
    public Dictionary<string, string> Args { get; set; } = null!;
}
