using LoggingService.Application.Authentication.Application;

namespace LoggingService.Application.Auth.KeyFactory;

public interface IApiKeyHasher
{
    (ApiKey key, string rawKey) Create(DateTime expireAtUtc);
    bool ValidateKey(ApiKey source, string apiKey);
}