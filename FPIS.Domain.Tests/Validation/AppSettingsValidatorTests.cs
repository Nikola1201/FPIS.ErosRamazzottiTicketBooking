using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class AppSettingsValidatorTests
{
    private static AppSettings Valid() => new()
    {
        Key = "MaxTickets",
        Value = "10"
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(AppSettingsValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => AppSettingsValidator.Validate(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_KeyEmpty_ReturnsError(string bad)
    {
        var s = Valid(); s.Key = bad;
        Assert.Contains(AppSettingsValidator.Validate(s),
            e => e.MemberNames.Contains(nameof(AppSettings.Key)));
    }

    [Fact]
    public void Validate_KeyTooLong_ReturnsError()
    {
        var s = Valid(); s.Key = new string('k', 101);
        Assert.Contains(AppSettingsValidator.Validate(s),
            e => e.MemberNames.Contains(nameof(AppSettings.Key)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ValueEmpty_ReturnsError(string bad)
    {
        var s = Valid(); s.Value = bad;
        Assert.Contains(AppSettingsValidator.Validate(s),
            e => e.MemberNames.Contains(nameof(AppSettings.Value)));
    }

    [Fact]
    public void Validate_ValueTooLong_ReturnsError()
    {
        var s = Valid(); s.Value = new string('v', 2001);
        Assert.Contains(AppSettingsValidator.Validate(s),
            e => e.MemberNames.Contains(nameof(AppSettings.Value)));
    }

    [Fact]
    public void Validate_EmptySettings_ReturnsErrorsForAllRequiredFields()
    {
        var errors = AppSettingsValidator.Validate(new AppSettings());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(AppSettings.Key)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(AppSettings.Value)));
    }
}
