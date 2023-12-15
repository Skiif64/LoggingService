using LoggingService.Domain.Base;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LoggingService.Application.Authentication.Application;
public class ApplicationCredentials : BaseEntity //TODO: private setters, ctor
{
    public string Name { get; set; } = null!;
    public byte[] ApiKeyHash { get; set; } = null!;
    public DateTime ExpireAtUtc { get; set; }
    public IList<string> CollectionNames { get; set; } = null!;
    private ApplicationCredentials()
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

    public bool IsApiKeyEqual(string apiKey)
    {
        var keyHash = SHA512.HashData(Encoding.UTF8.GetBytes(apiKey));
        return ApiKeyHash.SequenceEqual(keyHash);
    }

    public static ApplicationCredentials Create(string name, string apiKey, DateTime expireAtUtc)
    {
        //TODO: use HMACSHA512 or RFC2898
        var keyHash = SHA512.HashData(Encoding.UTF8.GetBytes(apiKey));
        return new ApplicationCredentials
        {
            Id = Guid.NewGuid(),
            CreatedAtUtc = DateTime.UtcNow,
            Name = name,
            ApiKeyHash = keyHash,
            ExpireAtUtc = expireAtUtc,
        };
    }
}
