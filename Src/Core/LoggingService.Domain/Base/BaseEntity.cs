namespace LoggingService.Domain.Base;
public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; protected set; }

    protected BaseEntity()
    {
        
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if(obj is not BaseEntity other)
        {
            return false;
        }

        return Equals(other);
    }

    public bool Equals(BaseEntity? other)
    {
        if(other is null)
        {
            return false;
        }

        return Id == other.Id;

    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
