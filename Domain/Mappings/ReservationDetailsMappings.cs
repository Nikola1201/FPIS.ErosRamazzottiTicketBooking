using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

public static class ReservationDetailsMappings
{
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
            var reserved = concertDate.Tickets.Count;
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
            IsGeneratedPromoCodeUsed = reservation.GeneratedPromoCode.IsUsed,
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
