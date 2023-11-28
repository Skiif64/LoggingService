using LoggingService.Domain.Shared;
using System.Linq.Expressions;

namespace LoggingService.Domain.Base;
public interface ISpecificationRepository<TEntity>
    where TEntity : BaseEntity
{
    IEnumerable<TEntity> FindAll(ISpecification<TEntity> specification);
    IEnumerable<TRespond> FindAll<TRespond>(ISpecification<TEntity> specification, Expression<Func<TEntity, TRespond>> selector)
        where TRespond : class;
    PagedList<TEntity> GetPaged(ISpecification<TEntity> specification, int pageIndex, int pageSize);
    TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate);
}
