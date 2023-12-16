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
    /// <param name="id">ApplicationIdentity id</param>
    /// <param name="expireAtUtc">New expiration time</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Result with new apiKey or error</returns>
    Task<Result<string>> RenewApplicationAsync(Guid id, DateTime expireAtUtc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke application identity
    /// </summary>
    /// <param name="id">Application id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> RevokeApplicationAsync(Guid id, CancellationToken cancellationToken = default);
}
