using LoggingService.Domain.Shared;
using MediatR;

namespace LoggingService.Application.Base.Messaging;
public interface IQueryHandler<TRequest, TResult> : IRequestHandler<TRequest, Result<TResult>>
    where TRequest : class, IQuery<TResult>
{
}
