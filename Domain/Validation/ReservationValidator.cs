using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="Reservation"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class ReservationValidator
{
    /// <summary>
    /// Validira <see cref="Reservation"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="reservation">Rezervacija koja se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="reservation"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);
        var errors = new List<ValidationResult>();

        if (reservation.Customer is null)
            errors.Add(new ValidationResult(
                "Customer je obavezan.",
                new[] { nameof(Reservation.Customer) }));

        if (reservation.AccessToken is null)
            errors.Add(new ValidationResult(
                "AccessToken je obavezan.",
                new[] { nameof(Reservation.AccessToken) }));

        if (!Enum.IsDefined<ReservationStatus>(reservation.Status))
            errors.Add(new ValidationResult(
                "Status mora biti definisana vrednost iz ReservationStatus.",
                new[] { nameof(Reservation.Status) }));

        if (reservation.Tickets is null || reservation.Tickets.Count == 0)
            errors.Add(new ValidationResult(
                "Tickets mora biti ne-null i sadržati najmanje jednu kartu.",
                new[] { nameof(Reservation.Tickets) }));

        if (reservation.Discounts is null)
            errors.Add(new ValidationResult(
                "Discounts mora biti ne-null (prazna kolekcija je dozvoljena).",
                new[] { nameof(Reservation.Discounts) }));

        return errors;
    }
}
