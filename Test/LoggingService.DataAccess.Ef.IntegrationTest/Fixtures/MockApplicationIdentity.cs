using LoggingService.Application.Authentication.Application;
using System.Security.Cryptography;
using System.Text;

namespace LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
public static class MockApplicationIdentity
{
    private const string Secret = "SomeStringThatMustBeNotHere";
    public static Application.Authentication.Application.ApplicationIdentity Create(string key)
    {
        var secretHash = SHA512.HashData(Encoding.UTF8.GetBytes(Secret));
        var keyHash = HMACSHA512.HashData(secretHash, Encoding.UTF8.GetBytes(key));
        var apiKey = new ApiKey("TestKey", keyHash, DateTime.UtcNow.AddYears(1));
        return Application.Authentication.Application.ApplicationIdentity.Create("TestApplication", apiKey);
    }
}
