using System.Net;

namespace LoggingService.Domain.Shared;
public readonly struct Error
{
    public ErrorType Type { get; }
    public string Name { get; }
    public string Description { get; }

    public Error(ErrorType type, string name, string description)
    {
        Type = type;
        Name = name;
        Description = description;
    }

    public override string ToString()
    {
        return $"{Name}: {Description}";
    }
}
