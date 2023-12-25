using System.Security.Claims;
using LoggingService.Domain.Base;

namespace LoggingService.Domain.Features.Applications;
public class ApplicationIdentity : BaseEntity //TODO: private setters, ctor, move to domain?
{
    public string Name { get; private set; } = null!;
    private ApplicationIdentity()
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
            CreatedAtUtc = DateTime.UtcNow,
            Name = name,
        };
    }
}
