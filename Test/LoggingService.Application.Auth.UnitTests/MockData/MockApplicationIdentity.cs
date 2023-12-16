using LoggingService.Application.Authentication.Application;
using System.Security.Cryptography;
using System.Text;

namespace LoggingService.Application.Auth.UnitTests.MockData;
public static class MockApplicationIdentity
{
    private const string Secret = "SomeStringThatMustBeNotHere";
    public static Authentication.Application.ApplicationIdentity Create(string key)
    {
        var secretHash = SHA512.HashData(Encoding.UTF8.GetBytes(Secret));
        var keyHash = HMACSHA512.HashData(secretHash, Encoding.UTF8.GetBytes(key));
        var apiKey = new ApiKey("TestKey", keyHash, DateTime.UtcNow.AddYears(1));
        return Authentication.Application.ApplicationIdentity.Create("TestApplication", apiKey);
    }
}
