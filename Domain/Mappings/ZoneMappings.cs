using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

public static class ZoneMappings
{
    public static ZoneViewModel ToViewModel(this Zone zone, int capacityRemaining)
    {
        return new ZoneViewModel
        {
            Id = zone.Id,
            Name = zone.Name,
            Capacity = zone.Capacity,
            Price = zone.Price,
            CapacityRemaining = capacityRemaining
        };
    }
}
