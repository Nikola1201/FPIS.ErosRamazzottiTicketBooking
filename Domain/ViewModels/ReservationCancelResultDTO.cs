namespace FPIS.Domain.ViewModels;

/// <summary>
/// DTO koji se vraća klijentu nakon pokušaja otkazivanja rezervacije; sadrži status otkazivanja.
/// </summary>
public class ReservationCancelResultDTO
{
    /// <summary>Identifikator rezervacije.</summary>
    public Guid ReservationId { get; set; }
    /// <summary>Da li je rezervacija uspešno otkazana.</summary>
    public bool Cancelled { get; set; }
    /// <summary>Tekstualna poruka koja opisuje rezultat otkazivanja.</summary>
    public string Message { get; set; } = string.Empty;
}
