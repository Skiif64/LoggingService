using LoggingService.Domain.Base;
using LoggingService.Domain.Shared;

namespace LoggingService.Domain.Features.LogEvents;
public interface ILogEventRepository : ICrudRepository<LogEvent>
{
    Task<PagedList<LogEvent>> GetPagedByCollectionIdAsync(Guid collectionId, int pageIndex, int pageSize, CancellationToken cancellationToken);
}
