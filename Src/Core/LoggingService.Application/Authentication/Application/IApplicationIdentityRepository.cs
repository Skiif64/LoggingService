using LoggingService.Domain.Base;

namespace LoggingService.Application.Authentication.Application;
public interface IApplicationIdentityRepository : ICrudRepository<ApplicationIdentity>
{
    Task<ApplicationIdentity?> GetByKeyPrefixAsync(string keyPrefix, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
