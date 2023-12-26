using AutoFixture;
using LoggingService.Tests.Shared.SpecimenBuilders;

namespace LoggingService.Tests.Shared.Extensions;
public static class FixtureExtensions
{
    public static IFixture FreezeConstructorParameters<T>(this IFixture fixture, object parameters)
    {
        var builder = new CustomConstructorBuilder<T>();
        foreach (var prop in parameters.GetType().GetProperties())
        {
            builder.Addparameter(prop.Name, prop.GetValue(parameters)!);
        }
        fixture.Customize<T>(x => builder);
        return fixture;
    }


}
