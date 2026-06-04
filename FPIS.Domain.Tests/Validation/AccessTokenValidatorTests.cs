using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class AccessTokenValidatorTests
{
    private static AccessToken Valid() => new()
    {
        ReservationId = Guid.NewGuid(),
        Value = "abcdef0123456789",
        IsActive = true
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(AccessTokenValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => AccessTokenValidator.Validate(null!));
    }

    [Fact]
    public void Validate_ReservationIdEmpty_ReturnsError()
    {
        var t = Valid(); t.ReservationId = Guid.Empty;
        Assert.Contains(AccessTokenValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(AccessToken.ReservationId)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_ValueEmpty_ReturnsError(string bad)
    {
        var t = Valid(); t.Value = bad;
        Assert.Contains(AccessTokenValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(AccessToken.Value)));
    }

    [Fact]
    public void Validate_ValueTooLong_ReturnsError()
    {
        var t = Valid(); t.Value = new string('a', 257);
        Assert.Contains(AccessTokenValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(AccessToken.Value)));
    }

    [Fact]
    public void Validate_EmptyToken_ReturnsErrorsForAllRequiredFields()
    {
        var errors = AccessTokenValidator.Validate(new AccessToken());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(AccessToken.ReservationId)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(AccessToken.Value)));
    }
}
