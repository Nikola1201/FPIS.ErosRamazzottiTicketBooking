using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;

namespace FPIS.Domain.Mappings;

/// <summary>
/// Ekstenzije za mapiranje <see cref="Zone"/> u <see cref="ZoneViewModel"/>.
/// </summary>
public static class ZoneMappings
{
    /// <summary>Mapira zonu u view model uz zadati preostali kapacitet.</summary>
    /// <param name="zone">Zona za mapiranje.</param>
    /// <param name="capacityRemaining">Broj preostalih (slobodnih) mesta u zoni.</param>
    /// <returns>View model zone.</returns>
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
