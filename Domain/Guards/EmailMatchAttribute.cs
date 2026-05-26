using FPIS.Domain.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Guards;


public class EmailMatchAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var reservation = (ReservationPostDTO)validationContext.ObjectInstance;
        if (reservation != null && reservation.Customer.Email != reservation.Customer.ConfirmedEmail)
        {
            return new ValidationResult("Confirmed email must match email.", new[] { nameof(CustomerCreateDTO.ConfirmedEmail) });
        }
        return ValidationResult.Success;
    }
}
