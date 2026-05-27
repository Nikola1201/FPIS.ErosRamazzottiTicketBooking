using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

/// <summary>
/// Ekstenzije za mapiranje <see cref="Concert"/> u odgovarajuće view modele.
/// </summary>
public static class ConcertMappings
{
    /// <summary>Mapira koncert u view model za prikaz na home page-u.</summary>
    /// <param name="concert">Koncert za mapiranje.</param>
    /// <returns>View model koji odgovara koncertu.</returns>
    public static ConcertViewModel ToViewModel(this Concert concert)
    {
        return new ConcertViewModel
        {
            Title = concert.Name,
            City = concert.City,
            Venue = concert.Venue,
            Address = concert.Address,
            AdditionalInfo = concert.AdditionalInfo,
            Dates = concert.Dates?.Select(d => d.Date).ToList() ?? new List<DateTime>(),
        };
    }
    /// <summary>Mapira koncert i prateće podatke u view model stranice rezervacije.</summary>
    /// <param name="concert">Koncert za mapiranje.</param>
    /// <param name="zones">Lista zona dostupnih za koncert.</param>
    /// <param name="tickets">Lista postojećih karata (za obračun preostalog kapaciteta).</param>
    /// <param name="appSettings">Konfiguracioni parametri aplikacije.</param>
    /// <returns>View model za stranicu rezervacije.</returns>
    public static ReservationPageViewModel ToReservationPageViewModel(
    this Concert concert,
    IEnumerable<Zone> zones,
    IEnumerable<ReservationTicket> tickets,
    IEnumerable<AppSettings> appSettings)
    {
        return new ReservationPageViewModel
        {
            Concert= concert.ToViewModel(),
            Dates = concert.Dates?.Select(date =>
                date.ToViewModel(zones, tickets)
            ).ToList() ?? new List<ConcertDateViewModel>(),
            AppSettings = appSettings.ToDictionary(a => a.Key, a => a.Value)

        };
    }
}
