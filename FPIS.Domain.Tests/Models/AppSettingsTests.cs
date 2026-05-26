using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class AppSettingsTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new AppSettings().Id);

    [Fact]
    public void Key_DefaultsToEmptyString() => Assert.Equal(string.Empty, new AppSettings().Key);

    [Fact]
    public void Value_DefaultsToEmptyString() => Assert.Equal(string.Empty, new AppSettings().Value);

    [Theory]
    [InlineData("MaxTickets", "10")]
    [InlineData("PromoActive", "true")]
    [InlineData("Имена кирилицом", "значење")]
    public void KeyAndValue_RoundTrip(string key, string value)
    {
        var s = new AppSettings { Key = key, Value = value };
        Assert.Equal(key, s.Key);
        Assert.Equal(value, s.Value);
    }

    [Fact]
    public void Key_AcceptsVeryLongString()
    {
        var longString = new string('k', 5000);
        Assert.Equal(longString, new AppSettings { Key = longString }.Key);
    }
}
