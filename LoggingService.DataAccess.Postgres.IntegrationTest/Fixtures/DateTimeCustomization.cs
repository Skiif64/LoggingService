namespace LoggingService.DataAccess.Postgres.IntegrationTest.Fixtures;
public class DateTimeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() =>
        {
            return DateTimeOffset.FromUnixTimeSeconds(Random.Shared.Next(0, int.MaxValue)).UtcDateTime;
        });
    }
}
