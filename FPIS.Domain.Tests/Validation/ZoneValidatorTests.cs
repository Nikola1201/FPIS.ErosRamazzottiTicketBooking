using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class ZoneValidatorTests
{
    private static Zone Valid() => new()
    {
        Name = "VIP",
        Capacity = 100,
        Price = 50m
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(ZoneValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ZoneValidator.Validate(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NameEmpty_ReturnsError(string bad)
    {
        var z = Valid(); z.Name = bad;
        Assert.Contains(ZoneValidator.Validate(z),
            e => e.MemberNames.Contains(nameof(Zone.Name)));
    }

    [Fact]
    public void Validate_NameTooLong_ReturnsError()
    {
        var z = Valid(); z.Name = new string('a', 101);
        Assert.Contains(ZoneValidator.Validate(z),
            e => e.MemberNames.Contains(nameof(Zone.Name)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Validate_CapacityZeroOrNegative_ReturnsError(int bad)
    {
        var z = Valid(); z.Capacity = bad;
        Assert.Contains(ZoneValidator.Validate(z),
            e => e.MemberNames.Contains(nameof(Zone.Capacity)));
    }

    [Fact]
    public void Validate_PriceNegative_ReturnsError()
    {
        var z = Valid(); z.Price = -0.01m;
        Assert.Contains(ZoneValidator.Validate(z),
            e => e.MemberNames.Contains(nameof(Zone.Price)));
    }

    [Fact]
    public void Validate_PriceZero_NoError()
    {
        var z = Valid(); z.Price = 0m;
        Assert.DoesNotContain(ZoneValidator.Validate(z),
            e => e.MemberNames.Contains(nameof(Zone.Price)));
    }

    [Fact]
    public void Validate_EmptyZone_ReturnsErrorsForAllRequiredFields()
    {
        var errors = ZoneValidator.Validate(new Zone());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Zone.Name)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Zone.Capacity)));
    }
}
