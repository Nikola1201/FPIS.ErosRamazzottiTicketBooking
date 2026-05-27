using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

/// <summary>
/// Ekstenzije za mapiranje <see cref="ConcertDate"/> u <see cref="ConcertDateViewModel"/>.
/// </summary>
public static class ConcertDateMappings
{
    /// <summary>Mapira datum koncerta u view model sa raspoloživim zonama i preostalim kapacitetom.</summary>
    /// <param name="concertDate">Datum koncerta za mapiranje.</param>
    /// <param name="zones">Lista svih zona dostupnih za koncert.</param>
    /// <param name="tickets">Lista postojećih karata (za izračun preostalog kapaciteta po zoni).</param>
    /// <returns>View model datuma koncerta sa zonama.</returns>
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
