using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="ReservationTicket"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class ReservationTicketValidator
{
    /// <summary>
    /// Validira <see cref="ReservationTicket"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="ticket">Karta koja se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="ticket"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(ReservationTicket ticket)
    {
        ArgumentNullException.ThrowIfNull(ticket);
        var errors = new List<ValidationResult>();

        if (ticket.ReservationId == Guid.Empty)
            errors.Add(new ValidationResult(
                "ReservationId je obavezan i ne sme biti Guid.Empty.",
                new[] { nameof(ReservationTicket.ReservationId) }));

        if (ticket.ZoneId == Guid.Empty)
            errors.Add(new ValidationResult(
                "ZoneId je obavezan i ne sme biti Guid.Empty.",
                new[] { nameof(ReservationTicket.ZoneId) }));

        if (ticket.ConcertDateId == Guid.Empty)
            errors.Add(new ValidationResult(
                "ConcertDateId je obavezan i ne sme biti Guid.Empty.",
                new[] { nameof(ReservationTicket.ConcertDateId) }));

        if (ticket.Price < 0m)
            errors.Add(new ValidationResult(
                "Price ne sme biti negativan.",
                new[] { nameof(ReservationTicket.Price) }));

        return errors;
    }
}
