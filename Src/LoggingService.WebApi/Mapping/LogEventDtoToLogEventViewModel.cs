using LoggingService.Application.Features.LogEvents;
using LoggingService.WebApi.Contracts.Models;
using Mapster;

namespace LoggingService.WebApi.Mapping;

public class LogEventDtoToLogEventViewModel : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LogEventDto, LogEventViewModel>();
    }
}
