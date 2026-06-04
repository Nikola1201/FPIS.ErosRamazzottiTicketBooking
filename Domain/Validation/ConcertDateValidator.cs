using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="ConcertDate"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class ConcertDateValidator
{
    /// <summary>
    /// Validira <see cref="ConcertDate"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="concertDate">Datum koncerta koji se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="concertDate"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(ConcertDate concertDate)
    {
        ArgumentNullException.ThrowIfNull(concertDate);
        var errors = new List<ValidationResult>();

        if (concertDate.Date == default)
            errors.Add(new ValidationResult(
                "Date je obavezan i ne sme biti default(DateTime).",
                new[] { nameof(ConcertDate.Date) }));

        if (concertDate.ConcertId == Guid.Empty)
            errors.Add(new ValidationResult(
                "ConcertId je obavezan i ne sme biti Guid.Empty.",
                new[] { nameof(ConcertDate.ConcertId) }));

        return errors;
    }
}
