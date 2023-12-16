using LoggingService.Application.Authentication.Application;
using Microsoft.EntityFrameworkCore;

namespace LoggingService.DataAccess.Ef.Repositories;
internal sealed class ApplicationIdentityRepository : BaseRepository<ApplicationIdentity>, IApplicationIdentityRepository
{
    public ApplicationIdentityRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await Context.Applications.AnyAsync(app => app.Name == name, cancellationToken);
    }

    public async Task<ApplicationIdentity?> GetByKeyPrefixAsync(string keyPrefix, CancellationToken cancellationToken = default)
    {
        var identity = await Context.Applications
            .FirstOrDefaultAsync(app => app.ApiKey.ApiKeyPrefix == keyPrefix, cancellationToken);
        return identity;
    }
}
