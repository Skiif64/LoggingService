using LoggingService.Domain.Shared;
using System.Text.RegularExpressions;

namespace LoggingService.Domain.Features.LogEvents;
public static class LogEventValidation
{
    private const string MessageTokenRegex = @"{(\w|\d|-|_)+}"; //TODO: except \{ \}
    public static Result Validate(string messageTemplate, Dictionary<string, string> args)
    {
        var parsedTokens = ParseTokens(messageTemplate);
        if (parsedTokens.Count() != args.Count)
        {
            return Result.Failure(LogEventsErrors.ParseError);
        }
        foreach(var token in parsedTokens)
        {            
            if(!args.ContainsKey(token))
            {
                return Result.Failure(LogEventsErrors.ParseError);
            }
        }

        return Result.Success();
    }

    private static IEnumerable<string> ParseTokens(string template)
    {
        var regex = new Regex(MessageTokenRegex, RegexOptions.Compiled);
        var matches = regex.Matches(template);
        var tokens = matches.Select(match => match.Value[1..^1]);
        return tokens;
    }
}