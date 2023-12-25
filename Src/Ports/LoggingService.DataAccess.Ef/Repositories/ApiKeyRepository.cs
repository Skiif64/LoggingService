using LoggingService.Application.Authentication.Application;
using Microsoft.EntityFrameworkCore;

namespace LoggingService.DataAccess.Ef.Repositories;

public class ApiKeyRepository : BaseRepository<ApiKey>, IApiKeyRepository
{
    public ApiKeyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ApiKey?> GetByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ApiKey>()
            .SingleOrDefaultAsync(key => key.ApiKeyPrefix == prefix, cancellationToken);
    }
}