namespace LoggingService.Application.Authentication.Application;
public interface IApplicationAuthenticationService
{
    Task<ApplicationCredentials?> GetCredentialsByApiKeyAsync(string apiKey, CancellationToken cancellationToken = default);
    Task<string> RegisterApplicationAsync(string name, CancellationToken cancellationToken = default);
}
