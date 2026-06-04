using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class PromoCodeValidatorTests
{
    private static PromoCode Valid() => new()
    {
        Code = "ABCD123456",
        IsActive = true,
        IsUsed = false
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(PromoCodeValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => PromoCodeValidator.Validate(null!));
    }

    [Fact]
    public void Validate_CodeNull_ReturnsError()
    {
        var p = Valid(); p.Code = null!;
        Assert.Contains(PromoCodeValidator.Validate(p),
            e => e.MemberNames.Contains(nameof(PromoCode.Code)));
    }

    [Theory]
    [InlineData("")]
    [InlineData("ABC")]
    [InlineData("ABCDEFGHI")]
    [InlineData("ABCDEFGHIJK")]
    [InlineData("ABCD12345!")]
    [InlineData("ABCD 12345")]
    [InlineData("ABCDŠČĆŽĐ1")]
    public void Validate_CodeInvalid_ReturnsError(string bad)
    {
        var p = Valid(); p.Code = bad;
        Assert.Contains(PromoCodeValidator.Validate(p),
            e => e.MemberNames.Contains(nameof(PromoCode.Code)));
    }

    [Theory]
    [InlineData("1234567890")]
    [InlineData("ABCDEFGHIJ")]
    [InlineData("abcdefghij")]
    [InlineData("aB1cD2eF3g")]
    public void Validate_CodeValid_NoError(string good)
    {
        var p = Valid(); p.Code = good;
        Assert.DoesNotContain(PromoCodeValidator.Validate(p),
            e => e.MemberNames.Contains(nameof(PromoCode.Code)));
    }
}
