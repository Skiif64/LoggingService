using System.Text;
using Bogus;
using LoggingService.Application.Features.LogEvents.Commands;
using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Tests.Shared.Fakes;

public static partial class Fake
{
    public static class LogEvent
    {
        public static Faker<Domain.Features.LogEvents.LogEvent> CreateLogEventFaker(Guid? collectionId = null)
        {
            return new Faker<Domain.Features.LogEvents.LogEvent>()
                .CustomInstantiator(faker =>
                {
                    var properties = CreateProperties(faker);
                    var message = CreateMessage(faker, properties);
                    var logEvent = Domain.Features.LogEvents.LogEvent.Create(
                        faker.Date.Past(),
                        collectionId ?? faker.Random.Guid(),
                        faker.Random.Enum<LogEventLevel>(),
                        message,
                        properties
                    );
                    return logEvent.Value;
                });
        }

        public static Faker<CreateLogEventDto> CreateLogEventCreateDtoFaker()
        {
            return new Faker<CreateLogEventDto>()
                .CustomInstantiator(faker =>
                {
                    var properties = CreateProperties(faker);
                    var message = CreateMessage(faker, properties);
                    return new CreateLogEventDto(
                        faker.Random.Enum<LogEventLevel>(),
                        faker.Date.Past(),
                        message,
                        properties
                    );
                });
        }

        private static Dictionary<string, string> CreateProperties(Faker faker, int min = 1, int max = 3)
        {
            var dictionary = new Dictionary<string, string>();

            for (int i = 0; i < faker.Random.Int(min, max); i++)
            {
                dictionary.Add(faker.Lorem.Word() + i, faker.Lorem.Word());
            }
            
            return dictionary;
        }
        
        private static string CreateMessage(Faker faker, Dictionary<string, string> args)
        {
            var sb = new StringBuilder(faker.Lorem.Paragraph());
            var currentPosition = 0;
            foreach (var arg in args)
            {
                sb.Insert(currentPosition, $" {{{arg.Key}}} ");
                currentPosition += arg.Key.Length + 3;
            }

            return sb.ToString();
        }
    }
}