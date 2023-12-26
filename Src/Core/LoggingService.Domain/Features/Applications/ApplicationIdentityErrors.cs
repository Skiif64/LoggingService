using System.Net;
using LoggingService.Domain.Shared;

namespace LoggingService.Domain.Features.Applications;
public static class ApplicationIdentityErrors
{
    public static Error KeyError
        => new Error(HttpStatusCode.Unauthorized, "ApplicationIdentity.Key", "Api Key is invalid or application not found");

    public static Error NotFound
        => new Error(HttpStatusCode.NotFound, "ApplicationIdentity.NotFound", "Application not found");
    //TODO: pass argument name
    public static Error ArgumentError
        => new Error(HttpStatusCode.BadRequest, "ApplicationIdentity.Argument", "Argument was invalid");

    public static Error AlreadyExistsError(string name)
        => new Error(HttpStatusCode.BadRequest, "ApplicationIdentity.AlreadyExists", $"Application with name: {name} already exists");
}
