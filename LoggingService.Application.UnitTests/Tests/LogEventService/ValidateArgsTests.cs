using LoggingService.Domain.Features.LogEvents;

namespace LoggingService.Application.UnitTests.Tests.LogEventService;
public class ValidateArgsTests : TestBase
{
    private readonly ILogEventService _sut;

    public ValidateArgsTests()
    {
        _sut = new Domain.Features.LogEvents.LogEventService();
    }

    [Fact]
    public void Validate_ShouldReturnSuccessResult_WhenArgsMatchesTemplate()
    {
        var messageTemplate = "Test log: {person} stroking {gender} dick";
        var args = new Dictionary<string, string>
        {
            ["person"] = "Yan",
            ["gender"] = "Attack helicopter"
        };

        var result = _sut.Validate(messageTemplate, args);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldReturnSuccessResult_WhenArgsDoesNotContains()
    {
        var messageTemplate = "Test log with no args";
        var args = new Dictionary<string, string>();

        var result = _sut.Validate(messageTemplate, args);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenArgsDidNotMatchedTemplate()
    {
        var messageTemplate = "Test log: {person} stroking {gender} dick";
        var args = new Dictionary<string, string>
        {
            ["mann"] = "Yan",
            ["gender"] = "Attack helicopter"
        };

        var result = _sut.Validate(messageTemplate, args);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenArgsCountDidNotMatchedTemplate()
    {
        var messageTemplate = "Test log: {person} stroking {gender} dick";
        var args = new Dictionary<string, string>
        {
            ["mann"] = "Yan",
            ["gender"] = "Attack helicopter",
            ["semen"] = "20ml/s",
        };

        var result = _sut.Validate(messageTemplate, args);

        result.IsSuccess.Should().BeFalse();
    }
}
