namespace LoggingService.Domain.Base;
public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public BaseEntity(Guid id, DateTime createdAtUtc)
    {
        Id = id;
        CreatedAtUtc = createdAtUtc;
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

        return Id == other.Id
            && CreatedAtUtc == other.CreatedAtUtc;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, CreatedAtUtc);
    }
}
