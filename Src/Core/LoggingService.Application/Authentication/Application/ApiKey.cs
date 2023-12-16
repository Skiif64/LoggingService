namespace LoggingService.Application.Authentication.Application;

public sealed class ApiKey
{
    public string ApiKeyPrefix { get; private set; } = null!;
    public byte[] ApiKeyHash { get; private set; } = null!;
    public DateTime ExpireAtUtc { get; private set; }
    private ApiKey()
    {
        
    }
    public ApiKey(string prefix, byte[] hash, DateTime expireAtUtc)
    {
        ApiKeyPrefix = prefix;
        ApiKeyHash = hash;
        ExpireAtUtc = expireAtUtc;
    }
}
