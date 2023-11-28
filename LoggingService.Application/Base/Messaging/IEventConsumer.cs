using MediatR;

namespace LoggingService.Application.Base.Messaging;
public interface IEventConsumer<TEvent> : INotificationHandler<TEvent>
    where TEvent : class, IEvent
{
}
