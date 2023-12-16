using LoggingService.Application.Authentication.Application;
using LoggingService.Application.Base;
using LoggingService.Domain.Shared;
using LoggingService.Application.Errors;
using LoggingService.Application.Auth.KeyFactory;

namespace LoggingService.Application.Auth;
internal sealed class ApplicationAuthenticationService : IApplicationAuthenticationService
{
    private const string Secret = "SomeStringThatMustBeNotHere";//TODO: remove this shit
    private readonly IApplicationIdentityRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApiKeyHasher _keyHasher;

    public ApplicationAuthenticationService(IApplicationIdentityRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _keyHasher = new HMACSHA512ApiKeyHasher(Secret); //TODO: get from di
    }

    public async Task<Result<Authentication.Application.ApplicationIdentity>> GetApplicationAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        var keyPrefix = apiKey.Split(".")[0];
        var identity = await _repository.GetByKeyPrefixAsync(keyPrefix, cancellationToken);
        if(identity is null)
        {
            return Result.Failure<Authentication.Application.ApplicationIdentity>(ApplicationIdentityErrors.NotFound);
        }

        if(!_keyHasher.ValidateKey(identity.ApiKey, apiKey))
        {
            return Result.Failure<Authentication.Application.ApplicationIdentity>(ApplicationIdentityErrors.KeyError);
        }

        return Result.Success(identity);
    }

    public async Task<Result<string>> RegisterApplicationAsync(string name, DateTime expireAtUtc, CancellationToken cancellationToken = default)
    {
        if(await _repository.ExistsByNameAsync(name, cancellationToken))
        {
            return Result.Failure<string>(ApplicationIdentityErrors.AlreadyExistsError(name));
        }
        var (apiKey, rawKey) = _keyHasher.Create(expireAtUtc);
        var identity = Authentication.Application.ApplicationIdentity.Create(name, apiKey);

        await _repository.InsertAsync(identity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            return Result.Failure<string>(ApplicationErrors.SaveChangesError);
        }
        return Result.Success(rawKey);
    }

    public async Task<Result<string>> RenewApplicationAsync(Guid id, DateTime expireAtUtc, CancellationToken cancellationToken = default)
    {
        if(expireAtUtc < DateTime.UtcNow)
        {
            return Result.Failure<string>(ApplicationIdentityErrors.ArgumentError);
        }
        var identity = await _repository.GetByIdAsync(id, cancellationToken);
        if(identity is null)
        {
            return Result.Failure<string>(ApplicationIdentityErrors.NotFound);
        }
        var (newKey, rawKey) = _keyHasher.Create(expireAtUtc);
        identity.ApiKey = newKey;

        _repository.Update(identity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (_unitOfWork.SaveChangesException is not null)
        {
            return Result.Failure<string>(ApplicationErrors.SaveChangesError);
        }

        return Result.Success(rawKey);
    }

    public async Task<Result> RevokeApplicationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var identity = await _repository.GetByIdAsync(id, cancellationToken);
        if(identity is null)
        {
            return Result.Failure(ApplicationIdentityErrors.NotFound);
        }

        _repository.Delete(identity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if(_unitOfWork.SaveChangesException is not null)
        {
            return Result.Failure(ApplicationErrors.SaveChangesError);
        }

        return Result.Success();
    }
}
