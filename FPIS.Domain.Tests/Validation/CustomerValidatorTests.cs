using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class CustomerValidatorTests
{
    private static Customer Valid() => new()
    {
        FirstName = "Jovan",
        LastName = "Jovanović",
        Email = "jovan@example.com",
        ConfirmedEmail = "jovan@example.com",
        Address = "Glavna 1",
        City = "Beograd",
        PostalCode = "11000",
        Country = "Srbija"
    };

    [Fact]
    public void Validate_ValidCustomer_ReturnsEmpty()
    {
        var errors = CustomerValidator.Validate(Valid());
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_NullCustomer_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => CustomerValidator.Validate(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_FirstNameEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.FirstName = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.FirstName)));
    }

    [Fact]
    public void Validate_FirstNameTooLong_ReturnsError()
    {
        var c = Valid(); c.FirstName = new string('a', 101);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.FirstName)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_LastNameEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.LastName = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.LastName)));
    }

    [Fact]
    public void Validate_LastNameTooLong_ReturnsError()
    {
        var c = Valid(); c.LastName = new string('a', 101);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.LastName)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("not-an-email")]
    [InlineData("missing@dot")]
    [InlineData("@nope.com")]
    [InlineData("nope@.com")]
    public void Validate_EmailInvalid_ReturnsError(string bad)
    {
        var c = Valid(); c.Email = bad; c.ConfirmedEmail = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Email)));
    }

    [Fact]
    public void Validate_EmailTooLong_ReturnsError()
    {
        var local = new string('a', 250);
        var tooLong = $"{local}@x.io";
        var c = Valid(); c.Email = tooLong; c.ConfirmedEmail = tooLong;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Email)));
    }

    [Fact]
    public void Validate_ConfirmedEmailMismatch_ReturnsError()
    {
        var c = Valid(); c.ConfirmedEmail = "drugi@example.com";
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.ConfirmedEmail)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_AddressEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Address = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Address)));
    }

    [Fact]
    public void Validate_AddressTooLong_ReturnsError()
    {
        var c = Valid(); c.Address = new string('a', 201);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Address)));
    }

    [Fact]
    public void Validate_Address2NullOrUnset_NoError()
    {
        var c = Valid(); c.Address2 = null;
        var errors = CustomerValidator.Validate(c);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains(nameof(Customer.Address2)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_Address2PresentButEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Address2 = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Address2)));
    }

    [Fact]
    public void Validate_Address2TooLong_ReturnsError()
    {
        var c = Valid(); c.Address2 = new string('a', 201);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Address2)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_CityEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.City = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.City)));
    }

    [Fact]
    public void Validate_CityTooLong_ReturnsError()
    {
        var c = Valid(); c.City = new string('a', 101);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.City)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_PostalCodeEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.PostalCode = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.PostalCode)));
    }

    [Fact]
    public void Validate_PostalCodeTooLong_ReturnsError()
    {
        var c = Valid(); c.PostalCode = new string('1', 21);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.PostalCode)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_CountryEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Country = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Country)));
    }

    [Fact]
    public void Validate_CountryTooLong_ReturnsError()
    {
        var c = Valid(); c.Country = new string('a', 101);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Country)));
    }

    [Fact]
    public void Validate_PhoneNumberNull_NoError()
    {
        var c = Valid(); c.PhoneNumber = null;
        var errors = CustomerValidator.Validate(c);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains(nameof(Customer.PhoneNumber)));
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("abcd")]
    [InlineData("12")]
    [InlineData("12345678901234567890123456789012")]
    [InlineData("   ")]
    public void Validate_PhoneNumberInvalid_ReturnsError(string bad)
    {
        var c = Valid(); c.PhoneNumber = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.PhoneNumber)));
    }

    [Theory]
    [InlineData("+381601234567")]
    [InlineData("011 123 456")]
    [InlineData("123-456-7890")]
    public void Validate_PhoneNumberValid_NoError(string good)
    {
        var c = Valid(); c.PhoneNumber = good;
        var errors = CustomerValidator.Validate(c);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains(nameof(Customer.PhoneNumber)));
    }

    [Fact]
    public void Validate_CompanyNull_NoError()
    {
        var c = Valid(); c.Company = null;
        var errors = CustomerValidator.Validate(c);
        Assert.DoesNotContain(errors, e => e.MemberNames.Contains(nameof(Customer.Company)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_CompanyPresentButEmpty_ReturnsError(string bad)
    {
        var c = Valid(); c.Company = bad;
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Company)));
    }

    [Fact]
    public void Validate_CompanyTooLong_ReturnsError()
    {
        var c = Valid(); c.Company = new string('a', 101);
        var errors = CustomerValidator.Validate(c);
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Company)));
    }

    [Fact]
    public void Validate_EmptyCustomer_ReturnsErrorsForAllRequiredFields()
    {
        var errors = CustomerValidator.Validate(new Customer());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.FirstName)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.LastName)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Email)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Address)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.City)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.PostalCode)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Customer.Country)));
    }
}
