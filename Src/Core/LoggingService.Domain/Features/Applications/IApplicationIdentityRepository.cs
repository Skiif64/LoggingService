using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.Applications;
public interface IApplicationIdentityRepository : ICrudRepository<ApplicationIdentity>
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
