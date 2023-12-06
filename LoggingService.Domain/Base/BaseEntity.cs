namespace LoggingService.Domain.Base;
public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }

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
