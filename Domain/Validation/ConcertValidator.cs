using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="Concert"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class ConcertValidator
{
    /// <summary>Vraća listu grešaka validacije za zadati koncert; prazna lista znači da je model validan.</summary>
    /// <param name="concert">Koncert za validaciju.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka (prazna ako je validan).</returns>
    /// <exception cref="ArgumentNullException">Bacama ako je <paramref name="concert"/> null.</exception>
    public static IReadOnlyList<ValidationResult> Validate(Concert concert)
    {
        ArgumentNullException.ThrowIfNull(concert);
        var errors = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(concert.Name) || concert.Name.Length > 200)
            errors.Add(new ValidationResult(
                "Name je obavezan, dužina 1–200 znakova.",
                new[] { nameof(Concert.Name) }));

        if (string.IsNullOrWhiteSpace(concert.City) || concert.City.Length > 100)
            errors.Add(new ValidationResult(
                "City je obavezan, dužina 1–100 znakova.",
                new[] { nameof(Concert.City) }));

        if (string.IsNullOrWhiteSpace(concert.Venue) || concert.Venue.Length > 200)
            errors.Add(new ValidationResult(
                "Venue je obavezan, dužina 1–200 znakova.",
                new[] { nameof(Concert.Venue) }));

        if (string.IsNullOrWhiteSpace(concert.Address) || concert.Address.Length > 200)
            errors.Add(new ValidationResult(
                "Address je obavezan, dužina 1–200 znakova.",
                new[] { nameof(Concert.Address) }));

        if (concert.AdditionalInfo is null || concert.AdditionalInfo.Length > 2000)
            errors.Add(new ValidationResult(
                "AdditionalInfo mora biti ne-null i dužine ≤ 2000 znakova (prazna vrednost je dozvoljena).",
                new[] { nameof(Concert.AdditionalInfo) }));

        return errors;
    }
}
