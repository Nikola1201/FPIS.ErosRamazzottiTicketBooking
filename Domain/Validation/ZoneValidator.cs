using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="Zone"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class ZoneValidator
{
    /// <summary>
    /// Validira <see cref="Zone"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="zone">Zona koja se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="zone"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(Zone zone)
    {
        ArgumentNullException.ThrowIfNull(zone);
        var errors = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(zone.Name) || zone.Name.Length > 100)
            errors.Add(new ValidationResult(
                "Name je obavezan, dužina 1–100 znakova.",
                new[] { nameof(Zone.Name) }));

        if (zone.Capacity <= 0)
            errors.Add(new ValidationResult(
                "Capacity mora biti veće od 0.",
                new[] { nameof(Zone.Capacity) }));

        if (zone.Price < 0m)
            errors.Add(new ValidationResult(
                "Price ne sme biti negativan.",
                new[] { nameof(Zone.Price) }));

        return errors;
    }
}
