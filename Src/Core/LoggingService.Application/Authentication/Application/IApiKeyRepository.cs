using LoggingService.Domain.Base;

namespace LoggingService.Application.Authentication.Application;

public interface IApiKeyRepository : ICrudRepository<ApiKey>
{
    Task<ApiKey?> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default);
}