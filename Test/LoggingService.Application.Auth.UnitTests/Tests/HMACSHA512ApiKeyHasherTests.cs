using FluentAssertions;
using LoggingService.Application.Auth.KeyFactory;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;
using System.Security.Cryptography;
using System.Text;

namespace LoggingService.Application.Auth.UnitTests.Tests;
public sealed class HMACSHA512ApiKeyHasherTests : TestBase
{
    private const string Secret = "SomeStringThatMustBeSecret";

    private readonly HMACSHA512ApiKeyHasher _sut;

    public HMACSHA512ApiKeyHasherTests()
    {
        _sut = new HMACSHA512ApiKeyHasher(Secret);
    }

    [Fact]
    public void Create_ShouldReturnValidRawKey()
    {
        var (key, rawKey) = _sut.Create(DateTime.UtcNow);

        Validate(key, rawKey).Should().BeTrue("raw key not passing validation");
    }

    [Fact]
    public void Validate_ShouldReturnTrue_WhenKeyIsValid()
    {
        var key = "SomeKey.RamdomData";
        var keyHash = HashKey(key);
        var apiKey = new ApiKey("SomeKey", keyHash, DateTime.UtcNow);

        var result = _sut.ValidateKey(apiKey, key);

        result.Should().BeTrue("raw key not pass validation");
    }

    [Fact]
    public void Validate_ShouldReturnFalse_WhenKeyIsInvalid()
    {
        var key = "SomeKey.RamdomData";
        var keyHash = HashKey(key);
        var apiKey = new ApiKey("SomeKey", keyHash, DateTime.UtcNow);

        var result = _sut.ValidateKey(apiKey, "SomeInvalid.Key");

        result.Should().BeFalse("raw key should not pass validation");
    }

    private bool Validate(ApiKey key, string rawKey)
    {
        var apiKeyHash = HashKey(rawKey);
        return key.ApiKeyHash.SequenceEqual(apiKeyHash);
    }

    private byte[] HashKey(string apiKey)
    {
        var secretHash = SHA512.HashData(Encoding.UTF8.GetBytes(Secret));
        var apiKeyHash = HMACSHA512.HashData(secretHash, Encoding.UTF8.GetBytes(apiKey));
        return apiKeyHash;
    }
}
