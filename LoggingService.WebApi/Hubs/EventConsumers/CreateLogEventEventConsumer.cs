using LoggingService.Application.Base.Messaging;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;

namespace LoggingService.WebApi.Hubs.EventConsumers;

internal sealed class CreateLogEventEventConsumer : IEventConsumer<LogEventCreatedEvent>
{
    private readonly NotificationHub _hub;
    private readonly IMapper _mapper;

    public CreateLogEventEventConsumer(NotificationHub hub, IMapper mapper)
    {
        _hub = hub;
        _mapper = mapper;
    }

    public async Task Handle(LogEventCreatedEvent notification, CancellationToken cancellationToken)
    {
        var mappedLogs = _mapper.Map<IEnumerable<LogEventViewModel>>(notification.Logs);
        await _hub.NotifyEventLogsAdd(notification.CollectionName, mappedLogs);
    }
}
