using LoggingService.WebApi.Contracts.Models;

namespace LoggingService.WebApi.Contracts.Hubs;

public interface INotificationHubClient
{
    Task SendLogEvents(IEnumerable<LogEventViewModel> logs);
    Task SendCollection(string collectionName);
}
