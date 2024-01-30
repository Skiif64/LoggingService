using System.Security.Cryptography;
using System.Text;
using LoggingService.Domain.Base;

namespace LoggingService.Application.Authentication.Application;

public sealed class ApiKey : BaseEntity
{
    public string ApiKeyPrefix { get; private set; } = null!;
    public byte[] ApiKeyHash { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime ExpireAtUtc { get; private set; }
    public Guid ApplicationId { get; private set; }
    private ApiKey()
    {
        
    }
    public static ApiKey Create(Guid applicationId, string prefix, string secret, DateTime expireAtUtc)
    {
        var hash = SHA512.HashData(Encoding.UTF8.GetBytes(secret));//TODO: move somewhere
        return new ApiKey
        {
            Id = Guid.NewGuid(),
            ApiKeyPrefix = prefix,
            ApiKeyHash = hash,
            CreatedAtUtc = DateTime.UtcNow,
            ExpireAtUtc = expireAtUtc,
            ApplicationId = applicationId,
        };
    }

    public bool ValidateSecret(string secret)
    {
        var otherHash = SHA512.HashData(Encoding.UTF8.GetBytes(secret));
        return ApiKeyHash.SequenceEqual(otherHash);
    }

    public void Renew(string newSecret)
    {
        var newHash = SHA512.HashData(Encoding.UTF8.GetBytes(newSecret));
        ApiKeyHash = newHash;
    }
}
