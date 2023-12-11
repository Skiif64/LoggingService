using LoggingService.Domain.Shared;
using LoggingService.WebApi.Contracts.Common;
using Mapster;

namespace LoggingService.WebApi.Mapping;

public class PagedListToPagedListViewModelConverter : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        var sourceType = typeof(PagedList<>);
        var destinationType = typeof(PagedListViewModel<>);
        config.ForType(sourceType, destinationType);
    }
}
