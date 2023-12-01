using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.LogEvents;
public class LogEvent : BaseEntity
{
    public DateTime Timestamp { get; private set; }
    public Guid CollectionId { get; private set; }
    public LogEventLevel LogLevel { get; private set; }
    public string Message { get; private set; }
    public Dictionary<string, string> Args { get; private set; }
    public LogEvent(Guid id,
                    DateTime createdAtUtc,
                    DateTime timestamp,
                    Guid collectionId,
                    LogEventLevel logLevel,
                    string message,
                    Dictionary<string, string> args) 
        : base(id, createdAtUtc)
    {
        Timestamp = timestamp;
        CollectionId = collectionId;
        LogLevel = logLevel;
        Message = message;
        Args = args;
    }
}
