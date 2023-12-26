using LoggingService.Domain.Shared;

namespace LoggingService.Application.Base.Messaging;
public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent message, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent;
}
