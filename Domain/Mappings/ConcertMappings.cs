using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

public static class ConcertMappings
{
    public static ConcertViewModel ToViewModel(this Concert concert)
    {
        return new ConcertViewModel
        {
            Name = concert.Name,
            City = concert.City,
            Venue = concert.Venue,
            Address = concert.Address,
            AdditionalInfo = concert.AdditionalInfo,
            Dates = concert.Dates?.Select(d => d.Date).ToList() ?? new List<DateTime>()
        };
    }
}
