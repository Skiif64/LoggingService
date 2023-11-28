using LoggingService.Domain.Shared;
using MediatR;

namespace LoggingService.Application.Base.Messaging;
public interface ICommand : IRequest<Result>
{
}
