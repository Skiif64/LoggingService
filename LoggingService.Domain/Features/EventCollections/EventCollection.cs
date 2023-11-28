using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.EventCollections;
public class EventCollection : BaseEntity
{
    public string Name { get; set; }
    public EventCollection(Guid id, DateTime createdAtUtc, string name) 
        : base(id, createdAtUtc)
    {
        Name = name;
    }
}
