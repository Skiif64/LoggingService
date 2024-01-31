using LoggingService.Domain.Base;
using LoggingService.Domain.Shared;

namespace LoggingService.Domain.Features.LogEvents;
public class LogEvent : BaseEntity
{
    public DateTime Timestamp { get; private set; }
    public Guid CollectionId { get; private set; }
    public LogEventLevel LogLevel { get; private set; }
    public string MessageTemplate { get; private set; } = null!;
    public Dictionary<string, string> Properties { get; private set; } = null!;

    private LogEvent() : base()
    {
        
    }

    public static Result<LogEvent> Create(
        DateTime timestamp, Guid collectionId, LogEventLevel logLevel, string message, Dictionary<string, string> args)
    {
        var validateResult = LogEventValidation.Validate(message, args);
        if(!validateResult.IsSuccess)
        {
            return Result.Failure<LogEvent>(validateResult.Error);
        }
        
        var logEvent = new LogEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = timestamp,
            CollectionId = collectionId,
            LogLevel = logLevel,
            MessageTemplate = message,
            Properties = args,
        };
        return Result.Success(logEvent);
    }
}
