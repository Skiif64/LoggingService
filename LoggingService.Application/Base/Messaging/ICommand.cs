using LoggingService.Domain.Shared;
using MediatR;

namespace LoggingService.Application.Base.Messaging;
public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{

}
