using FPIS.Domain.Guards;
using FPIS.Domain.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace FPIS.Domain.Tests.Guards;

public class EmailMatchAttributeTests
{
    // EmailMatchAttribute is applied at the class level on CustomerCreateDTO. It also supports
    // being invoked with a ReservationPostDTO context for legacy callers.
    private static ValidationResult? RunValidation(object instance)
    {
        var attr = new EmailMatchAttribute();
        var ctx = new ValidationContext(instance);
        var method = typeof(EmailMatchAttribute).GetMethod(
            "IsValid",
            BindingFlags.Instance | BindingFlags.NonPublic,
            new[] { typeof(object), typeof(ValidationContext) })!;
        return (ValidationResult?)method.Invoke(attr, new object?[] { instance, ctx });
    }

    [Fact]
    public void Matching_Emails_PassValidation()
    {
        var dto = new ReservationPostDTO
        {
            Customer = new CustomerCreateDTO
            {
                FirstName = "A", LastName = "B",
                Email = "a@b.rs", ConfirmedEmail = "a@b.rs",
                Address = "X", City = "Y", PostalCode = "Z", Country = "RS"
            },
            ConcertDateId = Guid.NewGuid(),
            Tickets = [ new TicketRequest { ZoneId = Guid.NewGuid(), Quantity = 1 } ]
        };

        var result = RunValidation(dto);

        // ValidationResult.Success is null
        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void Mismatching_Emails_FailValidation()
    {
        var dto = new ReservationPostDTO
        {
            Customer = new CustomerCreateDTO
            {
                FirstName = "A", LastName = "B",
                Email = "a@b.rs", ConfirmedEmail = "x@y.rs",
                Address = "X", City = "Y", PostalCode = "Z", Country = "RS"
            },
            ConcertDateId = Guid.NewGuid(),
            Tickets = [ new TicketRequest { ZoneId = Guid.NewGuid(), Quantity = 1 } ]
        };

        var result = RunValidation(dto);

        Assert.NotNull(result);
        Assert.Equal("Confirmed email must match email.", result!.ErrorMessage);
        Assert.Contains(nameof(CustomerCreateDTO.ConfirmedEmail), result.MemberNames);
    }

    [Fact]
    public void CustomerCreateDTOContext_MismatchingEmails_FailValidation()
    {
        // Regression: previously the attribute hard-cast to ReservationPostDTO and threw
        // InvalidCastException when invoked through Validator.TryValidateObject on a CustomerCreateDTO.
        var customer = new CustomerCreateDTO
        {
            FirstName = "A", LastName = "B",
            Email = "a@b.rs", ConfirmedEmail = "x@y.rs",
            Address = "X", City = "Y", PostalCode = "Z", Country = "RS"
        };

        var result = RunValidation(customer);

        Assert.NotNull(result);
        Assert.Contains(nameof(CustomerCreateDTO.ConfirmedEmail), result!.MemberNames);
    }

    [Fact]
    public void CustomerCreateDTOContext_MatchingEmails_PassValidation()
    {
        var customer = new CustomerCreateDTO
        {
            FirstName = "A", LastName = "B",
            Email = "a@b.rs", ConfirmedEmail = "a@b.rs",
            Address = "X", City = "Y", PostalCode = "Z", Country = "RS"
        };

        var result = RunValidation(customer);

        Assert.Equal(ValidationResult.Success, result);
    }

    [Fact]
    public void ValidatorTryValidateObject_PicksUpEmailMatchAttribute()
    {
        // Validator.TryValidateObject only runs class-level attributes when all property-level
        // attributes pass, so set PhoneNumber to a valid value to let [EmailMatch] surface.
        var customer = new CustomerCreateDTO
        {
            FirstName = "A", LastName = "B",
            Email = "a@b.rs", ConfirmedEmail = "MISMATCH@b.rs",
            PhoneNumber = "+381601234567",
            Address = "X", City = "Y", PostalCode = "Z", Country = "RS"
        };
        var ctx = new ValidationContext(customer);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(customer, ctx, results, validateAllProperties: true);

        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains(nameof(CustomerCreateDTO.ConfirmedEmail)));
    }
}
