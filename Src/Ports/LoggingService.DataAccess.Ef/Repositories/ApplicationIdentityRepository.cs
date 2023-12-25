using LoggingService.Application.Authentication.Application;
using LoggingService.Domain.Features.Applications;
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
}
