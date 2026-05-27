namespace FPIS.Domain.ViewModels;

/// <summary>
/// View model za prikaz osnovnih informacija o koncertu (naziv, lokacija, datumi).
/// </summary>
public class ConcertViewModel
{
    /// <summary>Naslov koncerta.</summary>
    public string? Title { get; set; }
    /// <summary>Grad u kojem se koncert održava.</summary>
    public string? City { get; set; }
    /// <summary>Naziv lokacije održavanja (dvorana, stadion).</summary>
    public string? Venue { get; set; }
    /// <summary>Adresa lokacije.</summary>
    public string? Address { get; set; }
    /// <summary>Lista datuma održavanja koncerta.</summary>
    public List<DateTime>? Dates { get; set; }
    /// <summary>Dodatne informacije o koncertu.</summary>
    public string? AdditionalInfo { get; set; }
}
