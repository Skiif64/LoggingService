using LoggingService.Domain.Shared;
using System.Text.RegularExpressions;

namespace LoggingService.Domain.Features.LogEvents;
public sealed class LogEventService : ILogEventService
{
    private const string MessageTokenRegex = @"{(\w|\d|-|_)+}"; //TODO: except \{ \}
    public Result Validate(string messageTemplate, Dictionary<string, string> args)
    {
        var parsedTokens = ParseTokens(messageTemplate);
        if(parsedTokens.Count() != args.Count)
        {
            return Result.Failure(LogEventsErrors.ParseError);
        }

        var joined = parsedTokens.Join(args.Keys, outer => outer, inner => inner,
            (outer, inner) => outer);

        if(joined.Count() != args.Count)
        {
            return Result.Success(LogEventsErrors.ParseError);
        }

        return Result.Success();
    }

    private static IEnumerable<string> ParseTokens(string template)
    {
        var regex = new Regex(MessageTokenRegex, RegexOptions.Compiled);
        var matches = regex.Matches(template);
        var tokens = matches.Select(match => match.Value[1..^2]);
        return tokens;
    }
}
