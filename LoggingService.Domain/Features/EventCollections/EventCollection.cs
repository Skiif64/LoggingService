using LoggingService.Domain.Base;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Domain.Features.EventCollections;
public class EventCollection : BaseEntity
{
    public string Name { get; set; }
    public IList<LogEvent> Events { get; set; } = new List<LogEvent>();
    public EventCollection(Guid id, DateTime createdAtUtc, string name) 
        : base(id, createdAtUtc)
    {
        Name = name;
    }
}
