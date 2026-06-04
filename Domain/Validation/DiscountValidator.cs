using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="Discount"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class DiscountValidator
{
    /// <summary>
    /// Validira <see cref="Discount"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="discount">Popust koji se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="discount"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(Discount discount)
    {
        ArgumentNullException.ThrowIfNull(discount);
        var errors = new List<ValidationResult>();

        if (!Enum.IsDefined<DiscountType>(discount.Type))
            errors.Add(new ValidationResult(
                "Type mora biti definisana vrednost iz DiscountType.",
                new[] { nameof(Discount.Type) }));

        if (discount.Percentage < 0m || discount.Percentage > 100m)
            errors.Add(new ValidationResult(
                "Percentage mora biti u opsegu 0–100 (uključivo).",
                new[] { nameof(Discount.Percentage) }));

        if (discount.ReservationId == Guid.Empty)
            errors.Add(new ValidationResult(
                "ReservationId je obavezan i ne sme biti Guid.Empty.",
                new[] { nameof(Discount.ReservationId) }));

        return errors;
    }
}
