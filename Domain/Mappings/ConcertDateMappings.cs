using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

public static class ConcertDateMappings
{
    public static ConcertDateViewModel ToViewModel(
        this ConcertDate concertDate,
        IEnumerable<Zone> zones,
        IEnumerable<ReservationTicket> tickets)
    {
        var zoneViewModels = zones.Select(zone =>
        {
            var reserved = tickets.Count(t => t.ConcertDateId == concertDate.Id && t.ZoneId == zone.Id);
            return zone.ToViewModel(zone.Capacity - reserved);
        }).ToList();

        return new ConcertDateViewModel
        {
            Id = concertDate.Id,
            Date = concertDate.Date,
            Zones = zoneViewModels,
        };
    }
}
