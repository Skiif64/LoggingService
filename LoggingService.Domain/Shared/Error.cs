using System.Net;

namespace LoggingService.Domain.Shared;
public readonly struct Error
{
    public int Code { get; }
    public string Name { get; }
    public string Description { get; }

    public Error(int code, string name, string description)
    {
        Code = code;
        Name = name;
        Description = description;
    }

    public Error(HttpStatusCode code, string name, string description)
    {
        Code = (int)code;
        Name = name;
        Description = description;
    }
}
