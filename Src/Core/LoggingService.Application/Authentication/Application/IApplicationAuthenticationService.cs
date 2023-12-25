using LoggingService.Domain.Features.Applications;
using LoggingService.Domain.Shared;

namespace LoggingService.Application.Authentication.Application;
public interface IApplicationAuthenticationService
{
    /// <summary>
    /// Get app credentials by apiKey and validate apiKey
    /// </summary>
    /// <param name="apiKey">apiKey</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with ApplicationCredentials or error</returns>
    Task<Result<ApplicationIdentity>> GetApplicationAsync(string apiKey, CancellationToken cancellationToken = default);
   /// <summary>
   /// Register new application
   /// </summary>
   /// <param name="name">application name</param>
   /// <param name="expireAtUtc">Time when apiKey is expire</param>
   /// <param name="cancellationToken"></param>
   /// <returns>Result with apiKey or error</returns>
    Task<Result<string>> RegisterApplicationAsync(string name, DateTime expireAtUtc, CancellationToken cancellationToken = default);
    /// <summary>
    /// Renew existing app credential
    /// </summary>
    /// <param name="keyPrefix">Api key prefix</param>
    /// <param name="expireAtUtc">New expiration time</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with new apiKey or error</returns>
    Task<Result<string>> RenewApiKeyAsync(string keyPrefix, DateTime expireAtUtc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke application identity
    /// </summary>
    /// <param name="keyPrefix">Api key prefix</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> RevokeApiKeyAsync(string keyPrefix, CancellationToken cancellationToken = default);
}
