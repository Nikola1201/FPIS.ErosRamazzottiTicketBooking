using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class DiscountValidatorTests
{
    private static Discount Valid() => new()
    {
        Type = DiscountType.EarlyBird,
        Percentage = 10m,
        ReservationId = Guid.NewGuid()
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(DiscountValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => DiscountValidator.Validate(null!));
    }

    [Fact]
    public void Validate_TypeUndefined_ReturnsError()
    {
        var d = Valid(); d.Type = (DiscountType)999;
        Assert.Contains(DiscountValidator.Validate(d),
            e => e.MemberNames.Contains(nameof(Discount.Type)));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(100.01)]
    [InlineData(1000)]
    public void Validate_PercentageOutOfRange_ReturnsError(double bad)
    {
        var d = Valid(); d.Percentage = (decimal)bad;
        Assert.Contains(DiscountValidator.Validate(d),
            e => e.MemberNames.Contains(nameof(Discount.Percentage)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50.5)]
    [InlineData(100)]
    public void Validate_PercentageAtBoundaries_NoError(double good)
    {
        var d = Valid(); d.Percentage = (decimal)good;
        Assert.DoesNotContain(DiscountValidator.Validate(d),
            e => e.MemberNames.Contains(nameof(Discount.Percentage)));
    }

    [Fact]
    public void Validate_ReservationIdEmpty_ReturnsError()
    {
        var d = Valid(); d.ReservationId = Guid.Empty;
        Assert.Contains(DiscountValidator.Validate(d),
            e => e.MemberNames.Contains(nameof(Discount.ReservationId)));
    }

    [Fact]
    public void Validate_EmptyDiscount_ReturnsErrorsForRequiredFields()
    {
        var errors = DiscountValidator.Validate(new Discount());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Discount.ReservationId)));
    }
}
