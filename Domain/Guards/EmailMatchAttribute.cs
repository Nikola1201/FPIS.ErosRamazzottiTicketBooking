using FPIS.Domain.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Guards;


/// <summary>
/// Validation atribut koji proverava da li se polja Email i ConfirmedEmail na <see cref="ReservationPostDTO.Customer"/>
/// poklapaju. Postavlja se na klasu <see cref="CustomerCreateDTO"/>.
/// </summary>
/// <remarks>
/// Greška se vraća uz oznaku polja <c>ConfirmedEmail</c>.
/// </remarks>
public class EmailMatchAttribute : ValidationAttribute
{
    /// <summary>Vraća uspeh ako se Email i ConfirmedEmail poklapaju, inače grešku validacije.</summary>
    /// <param name="value">Vrednost koja se validira (nije korišćena direktno — koristi se kontekst).</param>
    /// <param name="validationContext">Kontekst validacije; podržava <see cref="CustomerCreateDTO"/> ili <see cref="ReservationPostDTO"/>.</param>
    /// <returns>Rezultat validacije.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var customer = validationContext.ObjectInstance switch
        {
            CustomerCreateDTO c => c,
            ReservationPostDTO r => r.Customer,
            _ => null
        };

        if (customer != null && customer.Email != customer.ConfirmedEmail)
        {
            return new ValidationResult("Confirmed email must match email.", new[] { nameof(CustomerCreateDTO.ConfirmedEmail) });
        }
        return ValidationResult.Success;
    }
}
