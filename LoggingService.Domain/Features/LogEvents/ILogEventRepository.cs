using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.LogEvents;
public interface ILogEventRepository : ICrudRepository<LogEvent>, ISpecificationRepository<LogEvent>
{

}
