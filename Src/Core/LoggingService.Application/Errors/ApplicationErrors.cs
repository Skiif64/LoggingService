using LoggingService.Domain.Shared;
using System.Net;

namespace LoggingService.Application.Errors;
internal static class ApplicationErrors
{
    public static Error SaveChangesError
        => new Error(HttpStatusCode.InternalServerError, "Application.SaveChanges", "Unable to save changes to database");
}
