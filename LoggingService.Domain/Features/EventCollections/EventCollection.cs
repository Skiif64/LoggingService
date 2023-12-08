using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.EventCollections;
public class EventCollection : BaseEntity //TODO: private setters, ctor
{
    public string Name { get; set; } = null!;
    public Guid? ApplicationId { get; set; }
}
