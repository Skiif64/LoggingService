using System.Security.Cryptography;
using LoggingService.Application.Authentication.Application;
using LoggingService.Application.Base;
using LoggingService.Domain.Shared;
using LoggingService.Application.Errors;
using LoggingService.Domain.Features.Applications;
using ApplicationIdentity = LoggingService.Domain.Features.Applications.ApplicationIdentity;

namespace LoggingService.Application.Auth;
internal sealed class ApplicationAuthenticationService : IApplicationAuthenticationService
{
    private readonly IApplicationIdentityRepository _applicationRepository;
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApplicationAuthenticationService(IApplicationIdentityRepository applicationRepository,
        IApiKeyRepository apiKeyRepository,
        IUnitOfWork unitOfWork)
    {
        _applicationRepository = applicationRepository;
        _apiKeyRepository = apiKeyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ApplicationIdentity>> GetApplicationAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        var keyParts = apiKey.Split(".");
        if (keyParts.Length != 2)
        {
            return Result.Failure<ApplicationIdentity>(ApplicationIdentityErrors.KeyError);
        }
        var keyPrefix = keyParts[0];
        var keySecret = keyParts[1];
        var key = await _apiKeyRepository.GetByPrefixAsync(keyPrefix, cancellationToken);
        if (key is null)
        {
            return Result.Failure<ApplicationIdentity>(ApplicationIdentityErrors.NotFound); //TODO: move to key constants
        }
        if (!key.ValidateSecret(keySecret))
        {
            return Result.Failure<ApplicationIdentity>(ApplicationIdentityErrors.KeyError);
        }
        var identity = await _applicationRepository.GetByIdAsync(key.ApplicationId, cancellationToken);
        if(identity is null)
        {
            return Result.Failure<ApplicationIdentity>(ApplicationIdentityErrors.NotFound);
        }
        

        return Result.Success(identity);
    }

    public async Task<Result<string>> RegisterApplicationAsync(string name, DateTime expireAtUtc, CancellationToken cancellationToken = default)
    {
        if(await _applicationRepository.ExistsByNameAsync(name, cancellationToken))
        {
            return Result.Failure<string>(ApplicationIdentityErrors.AlreadyExistsError(name));
        }

        var prefix = GenerateRandomString(16);
        var secret = GenerateRandomString(32);
        var identity = ApplicationIdentity.Create(name);
        var apiKey = ApiKey.Create(identity.Id, prefix, secret, expireAtUtc);

        await _applicationRepository.InsertAsync(identity, cancellationToken);
        await _apiKeyRepository.InsertAsync(apiKey, cancellationToken);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            return Result.Failure<string>(ApplicationErrors.SaveChangesError);
        }

        var key = $"{prefix}.{secret}";
        return Result.Success(key);
    }

    public async Task<Result<string>> RenewApiKeyAsync(string keyPrefix, DateTime expireAtUtc, CancellationToken cancellationToken = default)
    {
        if(expireAtUtc < DateTime.UtcNow) //TODO: move validation
        {
            return Result.Failure<string>(ApplicationIdentityErrors.ArgumentError);
        }
        var key = await _apiKeyRepository.GetByPrefixAsync(keyPrefix, cancellationToken);
        if (key is null)
        {
            return Result.Failure<string>(ApplicationIdentityErrors.NotFound); // TODO: use from key errors
        }

        var newSecret = GenerateRandomString(32);
        key.Renew(newSecret);
        
        _apiKeyRepository.Update(key);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (_unitOfWork.SaveChangesException is not null)
        {
            return Result.Failure<string>(ApplicationErrors.SaveChangesError);
        }

        var newKey = $"{key.ApiKeyPrefix}.{newSecret}";
        return Result.Success(newKey);
    }
    //TODO: use not id
    public async Task<Result> RevokeApiKeyAsync(string keyPrefix, CancellationToken cancellationToken = default)
    {
        var key = await _apiKeyRepository.GetByPrefixAsync(keyPrefix, cancellationToken);
        if (key is null)
        {
            return Result.Failure(ApplicationIdentityErrors.NotFound);
        }
        _apiKeyRepository.Delete(key);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        return Result.Success();
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
