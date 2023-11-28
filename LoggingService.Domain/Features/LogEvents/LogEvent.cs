using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.LogEvents;
public class LogEvent : BaseEntity
{
    public DateTime Timestamp { get; private set; }
    public LogEventLevel LogLevel { get; private set; }
    public string Message { get; private set; }
    public Dictionary<string, string> Args { get; private set; }
    public LogEvent(Guid id,
                    DateTime createdAtUtc,
                    DateTime timestamp,
                    LogEventLevel logLevel,
                    string message,
                    Dictionary<string, string> args) 
        : base(id, createdAtUtc)
    {
        Timestamp = timestamp;
        LogLevel = logLevel;
        Message = message;
        Args = args;
    }
}
