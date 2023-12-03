using LoggingService.Application.Base.Messaging;
using MediatR;

namespace LoggingService.Application.EventBus;
public sealed class EventBus : IEventBus //TODO: remove
{
    private readonly IPublisher _publisher;

    public EventBus(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken)
        where TEvent : class, IEvent
    {
        await _publisher.Publish(message, cancellationToken);
    }
}
