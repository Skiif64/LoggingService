using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.EventCollections;
public class EventCollection : BaseEntity
{
    public string Name { get; private set; } = null!;
    public Guid? ApplicationId { get; private set; }

    private EventCollection() : base()
    {
        
    }

    public EventCollection(string name, Guid applicationId)
        : base(Guid.NewGuid())
    {
        Name = name;
        ApplicationId = applicationId;
    }
}
