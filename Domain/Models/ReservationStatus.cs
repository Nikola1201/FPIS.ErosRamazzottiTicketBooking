namespace FPIS.Domain.Models;

/// <summary>Status rezervacije u životnom ciklusu.</summary>
public enum ReservationStatus
{
    /// <summary>Rezervacija je aktivna i važeća.</summary>
    Active,
    /// <summary>Rezervacija je izmenjena u odnosu na originalno stanje.</summary>
    Modified,
    /// <summary>Rezervacija je otkazana i više nije važeća.</summary>
    Cancelled
}
