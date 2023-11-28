namespace LoggingService.Domain.Base;
public interface ISpecification<TEntity>
    where TEntity : BaseEntity
{
    IQueryable<TEntity> Apply(IQueryable<TEntity> queryable);
}
