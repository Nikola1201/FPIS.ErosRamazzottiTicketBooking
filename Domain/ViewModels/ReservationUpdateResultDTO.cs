namespace FPIS.Domain.ViewModels;

/// <summary>
/// DTO koji se vraća klijentu nakon pokušaja izmene rezervacije; sadrži status izmene i ažurirane podatke.
/// </summary>
public class ReservationUpdateResultDTO
{

    /// <summary>Identifikator rezervacije.</summary>
    public Guid ReservationId { get; set; }
    /// <summary>Pristupni token rezervacije.</summary>
    public string Token { get; set; } = string.Empty;
    /// <summary>Da li je rezervacija uspešno izmenjena.</summary>
    public bool Updated { get; set; }
    /// <summary>Ukupna cena rezervacije nakon izmene.</summary>
    public decimal TotalPrice { get; set; }
    /// <summary>Status rezervacije nakon izmene.</summary>
    public string Status { get; set; }
    /// <summary>Broj karata u rezervaciji nakon izmene.</summary>
    public int UpdatedTicketCount { get; set; }
    /// <summary>Tekstualna poruka koja opisuje rezultat izmene.</summary>
    public string Message { get; set; }
}
