namespace LoggingService.Application.Base;
public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    void BeginTransaction();
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
    void RollbackTransaction();
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    void CommitTransaction();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
