using LoggingService.Domain.Base;
using LoggingService.Domain.Exceptions;

namespace LoggingService.DataAccess.Postgres.Repositories;
internal abstract class BaseRepository<TEntity> : ICrudRepository<TEntity>
    where TEntity : BaseEntity
{
    protected ApplicationDbContext Context { get; }

    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingEntry = await Context.Set<TEntity>()
            .FindAsync(new object[] { id }, cancellationToken);
        if(existingEntry is null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id, "Id");
        }
        Context.Set<TEntity>().Remove(existingEntry);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken);

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }
}
