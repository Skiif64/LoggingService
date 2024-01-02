using System.Text;
using AutoFixture;
using AutoFixture.Kernel;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Tests.Shared.Specimen;

public sealed class LogEventSpecimenBuilder : ISpecimenBuilder
{
    private readonly Guid _collectionId;
    private readonly LogEventLevel _logLevel;

    public LogEventSpecimenBuilder(Guid? collectionId = null, LogEventLevel? logLevel = null)
    {
        _collectionId = collectionId ?? Guid.NewGuid();
        _logLevel = logLevel ?? (LogEventLevel)Random.Shared.Next(0, 6);
    }
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(LogEvent))
        {
            return new NoSpecimen();
        }
        var args = context.Create<Dictionary<string, string>>();
        var message = CreateMessage(context, args);
        return LogEvent.Create(
            context.Create<DateTime>(),
            _collectionId,
            _logLevel,
            message,
            args
        ).Value!;
    }
    
    private string CreateMessage(ISpecimenContext context, Dictionary<string, string> args)
    {
        var sb = new StringBuilder(context.Create<string>());
        foreach (var arg in args)
        {
            sb.Insert(sb.Length, $" {{{arg.Key}}} ");
        }

        return sb.ToString();
    }
}