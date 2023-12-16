using LoggingService.Application.Authentication.Application;
using System.Security.Cryptography;
using System.Text;

namespace LoggingService.Application.Auth.KeyFactory;
internal sealed class HMACSHA512ApiKeyHasher : IApiKeyHasher
{
    private readonly string _secret;

    public HMACSHA512ApiKeyHasher(string secret)
    {
        _secret = secret;
    }

    public (ApiKey key, string rawKey) Create(DateTime expireAtUtc)
    {
        var prefix = GenerateRandomString(16);
        var body = GenerateRandomString(32);
        var apiKey = $"{prefix}.{body}";

        var hash = HashKey(apiKey);

        return (new ApiKey(prefix, hash, expireAtUtc), apiKey);
    }

    public bool ValidateKey(ApiKey source, string apiKey)
    {
        var keyHash = HashKey(apiKey);
        return source.ApiKeyHash.SequenceEqual(keyHash);
    }

    private byte[] HashKey(string apiKey)
    {
        var secretHash = SHA512.HashData(Encoding.UTF8.GetBytes(_secret));
        var apiKeyHash = HMACSHA512.HashData(secretHash, Encoding.UTF8.GetBytes(apiKey));
        return apiKeyHash;
    }
       

    private static string GenerateRandomString(int length)
    {
        const string pool = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var randomBytes = RandomNumberGenerator.GetBytes(length);
        Span<char> temp = stackalloc char[length];
        for (int i = 0; i < length; i++)
        {
            var index = randomBytes[i] % pool.Length;
            temp[i] = pool[index];
        }

        return temp.ToString();

    }
}
