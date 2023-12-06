using LoggingService.Domain.Shared;

namespace LoggingService.Domain.Features.LogEvents;
public interface ILogEventService
{
    Result Validate(string messageTemplate, Dictionary<string, string> args);
}
