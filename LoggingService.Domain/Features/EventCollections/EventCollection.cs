using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.EventCollections;
public class EventCollection : BaseEntity
{
    public string Name { get; private set; }
    public Guid? ApplicationId { get; private set; }
    public EventCollection(Guid id, DateTime createdAtUtc, string name, Guid? applicationId) 
        : base(id, createdAtUtc)
    {
        Name = name;
        ApplicationId = applicationId;
    }
}
