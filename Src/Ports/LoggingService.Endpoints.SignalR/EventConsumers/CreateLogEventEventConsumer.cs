using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Endpoints.SignalR.Hubs;
using LoggingService.WebApi.Contracts.Hubs;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LoggingService.Endpoints.SignalR.EventConsumers;

internal sealed class CreateLogEventEventConsumer : IEventConsumer<LogEventCreatedEvent>
{
    private readonly IHubContext<NotificationHub, INotificationHubClient> _hub;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateLogEventEventConsumer> _logger;

    public CreateLogEventEventConsumer(IHubContext<NotificationHub, INotificationHubClient> hub, IMapper mapper, ILogger<CreateLogEventEventConsumer> logger)
    {
        _hub = hub;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(LogEventCreatedEvent notification, CancellationToken cancellationToken)
    {
        var mappedLogs = _mapper.Map<IEnumerable<LogEventViewModel>>(notification.Logs);
        await _hub.Clients.Group(notification.CollectionName).SendLogEvents(mappedLogs);
        _logger.LogInformation("Send {count} of log events to {collection} listeners", notification.Logs.Count(), notification.CollectionName);
    }
}
