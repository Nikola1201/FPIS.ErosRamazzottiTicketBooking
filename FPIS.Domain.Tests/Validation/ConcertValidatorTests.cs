using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class ConcertValidatorTests
{
    private static Concert Valid() => new()
    {
        Name = "Eros Ramazzotti — Tour 2026",
        City = "Beograd",
        Venue = "Štark Arena",
        Address = "Bulevar Arsenija Čarnojevića 58",
        AdditionalInfo = ""
    };

    [Fact]
    public void Validate_ValidConcert_ReturnsEmpty()
    {
        Assert.Empty(ConcertValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_NullConcert_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ConcertValidator.Validate(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NameEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Name = bad;
        var errors = ConcertValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Concert.Name)));
    }

    [Fact]
    public void Validate_NameTooLong_ReturnsError()
    {
        var c = Valid(); c.Name = new string('a', 201);
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.Name)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_CityEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.City = bad;
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.City)));
    }

    [Fact]
    public void Validate_CityTooLong_ReturnsError()
    {
        var c = Valid(); c.City = new string('a', 101);
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.City)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_VenueEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Venue = bad;
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.Venue)));
    }

    [Fact]
    public void Validate_VenueTooLong_ReturnsError()
    {
        var c = Valid(); c.Venue = new string('a', 201);
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.Venue)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_AddressEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Address = bad;
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.Address)));
    }

    [Fact]
    public void Validate_AddressTooLong_ReturnsError()
    {
        var c = Valid(); c.Address = new string('a', 201);
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.Address)));
    }

    [Fact]
    public void Validate_AdditionalInfoEmpty_NoError()
    {
        var c = Valid(); c.AdditionalInfo = "";
        Assert.DoesNotContain(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.AdditionalInfo)));
    }

    [Fact]
    public void Validate_AdditionalInfoTooLong_ReturnsError()
    {
        var c = Valid(); c.AdditionalInfo = new string('a', 2001);
        Assert.Contains(ConcertValidator.Validate(c),
            e => e.MemberNames.Contains(nameof(Concert.AdditionalInfo)));
    }

    [Fact]
    public void Validate_EmptyConcert_ReturnsErrorsForAllRequiredFields()
    {
        var errors = ConcertValidator.Validate(new Concert());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Concert.Name)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Concert.City)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Concert.Venue)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Concert.Address)));
    }
}
