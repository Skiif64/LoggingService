using LoggingService.Domain.Features.EventCollections;
using LoggingService.WebApi.Contracts.Models;
using Mapster;

namespace LoggingService.WebApi.Mapping;

public class EventCollectionToEventCollectionViewModelConverter : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EventCollection, EventCollectionViewModel>();
    }
}
