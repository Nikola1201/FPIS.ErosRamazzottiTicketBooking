using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

public static class ConcertMappings
{
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
    public static ReservationPageViewModel ToReservationPageViewModel(
    this Concert concert,
    IEnumerable<Zone> zones,
    IEnumerable<ReservationTicket> tickets,
    IEnumerable<AppSettings> appSettings)
    {
        return new ReservationPageViewModel
        {
            Concert= concert.ToViewModel(),
            Dates = concert.Dates.Select(date =>
                date.ToViewModel(zones, tickets)
            ).ToList(),
            AppSettings = appSettings.ToDictionary(a => a.Key, a => a.Value)

        };
    }
}
