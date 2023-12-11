using LoggingService.WebApi.Contracts.Hubs;
using LoggingService.WebApi.Contracts.Models;
using Microsoft.AspNetCore.SignalR;

namespace LoggingService.WebApi.Hubs;

public class NotificationHub : Hub<INotificationHubClient>
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public async Task ListenCollection(string collectionName, CancellationToken cancellationToken = default)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, collectionName, cancellationToken);
        _logger.LogInformation("Connection {id} has started listening {name}", Context.ConnectionId, collectionName); //TODO: get user claims
    }

    public async Task StopListenCollection(string collectionName, CancellationToken cancellationToken = default)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, collectionName, cancellationToken);
        _logger.LogInformation("Connection {id} has stopped listening {name}", Context.ConnectionId, collectionName);
    }

    public async Task NotifyEventLogsAdd(string collectionName, IEnumerable<LogEventViewModel> logs)
    {
        await Clients.Group(collectionName).SendLogEvents(logs);
        _logger.LogInformation("Send {count} of log events to {collection}", logs.Count(), collectionName);
    }
}
