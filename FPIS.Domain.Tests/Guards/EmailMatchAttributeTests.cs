using FPIS.Domain.Guards;
using FPIS.Domain.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace FPIS.Domain.Tests.Guards;

public class EmailMatchAttributeTests
{
    // EmailMatchAttribute is applied at the class level on CustomerCreateDTO, but its IsValid
    // method casts validationContext.ObjectInstance to ReservationPostDTO. So the only valid way
    // to exercise it is to invoke it directly with a ValidationContext bound to a ReservationPostDTO.
    private static ValidationResult? RunValidation(ReservationPostDTO dto)
    {
        var attr = new EmailMatchAttribute();
        var ctx = new ValidationContext(dto);
        // Invoke the protected IsValid(object?, ValidationContext) via reflection.
        var method = typeof(EmailMatchAttribute).GetMethod(
            "IsValid",
            BindingFlags.Instance | BindingFlags.NonPublic,
            new[] { typeof(object), typeof(ValidationContext) })!;
        return (ValidationResult?)method.Invoke(attr, new object?[] { dto.Customer, ctx });
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
        Assert.Contains("Confirmed email", result!.ErrorMessage);
    }
}
