using Bogus;

namespace LoggingService.Tests.Shared.Fakes;

public static partial class Fake
{
    public static class EventCollection
    {
        public static Faker<Domain.Features.EventCollections.EventCollection> CreateEventCollectionFaker(Guid? applicationId = null)
        {
            return new Faker<Domain.Features.EventCollections.EventCollection>()
                .CustomInstantiator(faker => new Domain.Features.EventCollections.EventCollection(
                    faker.Lorem.Word(),
                    applicationId ?? faker.Random.Guid()
                ));
        }
    }
}