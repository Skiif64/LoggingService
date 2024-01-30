using LoggingService.Application.Features.LogEvents;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.WebApi.Contracts.Models;
using Mapster;

namespace LoggingService.WebApi.Mapping;

public class LogEventToLogEventViewModel : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LogEvent, LogEventViewModel>();
    }
}
