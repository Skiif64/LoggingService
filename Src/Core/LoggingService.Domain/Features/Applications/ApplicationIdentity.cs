using System.Security.Claims;
using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.Applications;
public class ApplicationIdentity : BaseEntity
{
    public string Name { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }
    private ApplicationIdentity() : base()
    {
        
    }
    public Claim[] GetClaims()
    {
        return new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
            new Claim(ClaimTypes.Name, Name),
        };
    }

    public static ApplicationIdentity Create(string name)
    {       
        return new ApplicationIdentity
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
