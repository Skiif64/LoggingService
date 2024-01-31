using LoggingService.Domain.Shared;
using System.Net;

namespace LoggingService.Domain.Features.LogEvents;
public static class LogEventsErrors
{
    //TODO: normal error
    public static Error ParseError => new Error(ErrorType.Problem, "LogEvents.Parse", "Unable to parse log tokens");
}
