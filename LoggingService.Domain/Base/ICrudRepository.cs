namespace LoggingService.Domain.Base;
public interface ICrudRepository<TEntity>
    where TEntity : BaseEntity
{
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Update(TEntity entity);

}
