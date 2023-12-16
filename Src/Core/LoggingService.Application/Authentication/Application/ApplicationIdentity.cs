using LoggingService.Domain.Base;
using System.Security.Claims;

namespace LoggingService.Application.Authentication.Application;
public class ApplicationIdentity : BaseEntity //TODO: private setters, ctor, move to domain?
{
    public string Name { get; set; } = null!;
    public ApiKey ApiKey { get; set; } = null!;
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

    public static ApplicationIdentity Create(string name, ApiKey apiKey)
    {       
        return new ApplicationIdentity
        {
            Id = Guid.NewGuid(),
            CreatedAtUtc = DateTime.UtcNow,
            Name = name,
            ApiKey = apiKey,
        };
    }
}
