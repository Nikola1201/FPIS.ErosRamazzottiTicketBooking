using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

/// <summary>
/// Ekstenzije za mapiranje <see cref="Reservation"/> u <see cref="ReservationDetailsViewModel"/>.
/// </summary>
public static class ReservationDetailsMappings
{
    /// <summary>Mapira rezervaciju u detaljan view model sa kartama, popustima i informacijama o koncertu.</summary>
    /// <param name="reservation">Rezervacija za mapiranje.</param>
    /// <param name="concertDate">Datum koncerta na koji se rezervacija odnosi (može biti null).</param>
    /// <param name="zones">Lista svih zona (za obračun cene i preostalog kapaciteta).</param>
    /// <param name="allTickets">Lista svih karata (za obračun preostalog kapaciteta).</param>
    /// <returns>Detaljan view model rezervacije.</returns>
    public static ReservationDetailsViewModel ToReservationDetailsViewModel(
        this Reservation reservation,
        ConcertDate? concertDate,
        List<Zone> zones,
        List<ReservationTicket> allTickets
        )
    {
        var tickets = reservation.Tickets
            .Select(t => new TicketDetailsViewModel
            {
                TicketId = t.Id,
                ZoneName = zones.FirstOrDefault(z => z.Id == t.ZoneId)?.Name ?? "Unknown",
                Price = t.Price
            })
            .ToList();

        var discounts = reservation.Discounts
            .Select(d => new DiscountDetailsViewModel
            {
                Type = d.Type.ToString(),
                Percentage = d.Percentage
            })
            .ToList();

        var totalPrice = reservation.Tickets
            .GroupBy(t => t.ZoneId)
            .Sum(g =>
            {
                var zone = zones.FirstOrDefault(z => z.Id == g.Key);
                return (zone?.Price ?? 0m) * g.Count();
            });
        var finalPrice = tickets.Sum(t => t.Price);
        var zoneViewModels = zones.Select(zone =>
        {
            var reserved = concertDate?.Tickets?.Count ?? 0;
            return zone.ToViewModel(zone.Capacity - reserved + tickets.Count);
        }).ToList();

        return new ReservationDetailsViewModel
        {
            ReservationId = reservation.Id,
            Status = reservation.Status.ToString(),
            CustomerName = $"{reservation.Customer.FirstName} {reservation.Customer.LastName}",
            CustomerEmail = reservation.Customer.Email,
            AccessToken = reservation.AccessToken?.Value ?? string.Empty,
            UsedPromoCode = reservation.UsedPromoCode?.Code,
            GeneratedPromoCode = reservation.GeneratedPromoCode?.Code,
            IsGeneratedPromoCodeUsed = reservation.GeneratedPromoCode?.IsUsed ?? false,
            ZonesDetails = zoneViewModels,
            Tickets = tickets,
            Discounts = discounts,
            ConcertDate = concertDate?.Date,
            ConcertName = concertDate?.Concert?.Name,
            ConcertVenue = concertDate?.Concert?.Venue,
            ConcertCity = concertDate?.Concert?.City,
            TotalPrice = totalPrice,
            FinalPrice = finalPrice

        };
    }
}
