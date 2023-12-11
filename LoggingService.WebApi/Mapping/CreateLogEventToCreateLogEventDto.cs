using LoggingService.Application.Features.LogEvents.Commands;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.WebApi.Contracts.Models;
using Mapster;

namespace LoggingService.WebApi.Mapping;

public class CreateLogEventToCreateLogEventDto : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateLogEventViewModel, CreateLogEventDto>()
            .ConstructUsing(src 
            => new CreateLogEventDto(
                    src.LogLevel.Adapt<LogEventLevel>(),
                    src.Timestamp,
                    src.Message,
                    src.Args));
    }
}
