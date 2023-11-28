using LoggingService.Domain.Shared;
using MediatR;

namespace LoggingService.Application.Base.Messaging;
public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : class, ICommand
{
}
