using LoggingService.Domain.Shared;
using LoggingService.WebApi.Contracts.Common;
using Mapster;

namespace LoggingService.WebApi.Mapping;

public class ErrorToErrorViewModelConverter : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Error, ErrorViewModel>()
            .Map(src => src.Name, dest => dest.Name)
            .Map(src => src.Description, dest => dest.Description);
    }
}
