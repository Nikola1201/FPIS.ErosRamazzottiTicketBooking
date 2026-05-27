namespace FPIS.Domain.ViewModels;

/// <summary>
/// View model za prikaz selekcije zone na stranici rezervacije; sadrži kapacitet i broj slobodnih mesta.
/// </summary>
public class ZoneSelectionViewModel
{
    /// <summary>Identifikator zone.</summary>
    public Guid ZoneId { get; set; }
    /// <summary>Naziv zone (npr. "VIP", "Parter").</summary>
    public string ZoneName { get; set; } = string.Empty;
    /// <summary>Cena jedne karte u zoni.</summary>
    public decimal Price { get; set; }
    /// <summary>Broj još uvek slobodnih mesta u zoni.</summary>
    public int AvailableSeats { get; set; }
    /// <summary>Broj mesta koje je trenutno korisnik selektovao.</summary>
    public int SelectedSeats { get; set; }
    /// <summary>Ukupan kapacitet zone.</summary>
    public int TotalSeats { get; set; }

}
