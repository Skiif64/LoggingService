using LoggingService.Domain.Base;
using LoggingService.Domain.Shared;

namespace LoggingService.Domain.Features.LogEvents;
public class LogEvent : BaseEntity
{
    public DateTime Timestamp { get; set; }
    public Guid CollectionId { get; set; }
    public LogEventLevel LogLevel { get; set; }
    public string Message { get; set; } = null!;
    public Dictionary<string, string> Args { get; set; } = null!;

    private LogEvent()
    {
        
    }

    public static Result<LogEvent> Create(
        DateTime timestamp, Guid collectionId, LogEventLevel logLevel, string message, Dictionary<string, string> args)
    {
        var validateResult = LogEventValidation.Validate(message, args);
        if(!validateResult.IsSuccess)
        {
            return Result.Failure<LogEvent>(validateResult.Error!.Value);
        }
        var logEvent = new LogEvent
        {
            Id = Guid.NewGuid(),
            CreatedAtUtc = DateTime.UtcNow,
            Timestamp = timestamp,
            CollectionId = collectionId,
            LogLevel = logLevel,
            Message = message,
            Args = args,
        };
        return Result.Success(logEvent);
    }
}
