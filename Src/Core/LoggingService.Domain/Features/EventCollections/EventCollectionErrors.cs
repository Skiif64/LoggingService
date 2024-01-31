using LoggingService.Domain.Shared;
using System.Net;

namespace LoggingService.Domain.Features.EventCollections;
public static class EventCollectionErrors
{
    private const string EntityName = nameof(EventCollection);
    public static Error NotFound(string paramName, object param)
        => new Error(ErrorType.NotFound, "EventCollection.NotFound",
            $"EventCollection with {paramName} = {param} not found.");

    public static Error Duplicate(string paramName, object param)
        => new Error(ErrorType.Conflict, "EventCollection.Duplicate",
            $"EventCollection with {paramName} = {param} already exists");
}
