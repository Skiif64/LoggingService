﻿using LoggingService.WebApi.Contracts.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LoggingService.Endpoints.SignalR.Hubs;
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
}