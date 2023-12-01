using LoggingService.Domain.Base;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Domain.Features.EventCollections;
public class EventCollection : BaseEntity
{
    public string Name { get; private set; }
    public Guid? ApplicationId { get; private set; }
    public IList<LogEvent> Events { get; private set; } = new List<LogEvent>();
    public EventCollection(Guid id, DateTime createdAtUtc, string name, Guid? applicationId) 
        : base(id, createdAtUtc)
    {
        Name = name;
        ApplicationId = applicationId;
    }
}
