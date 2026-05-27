namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja koncert sa osnovnim informacijama (naziv, mesto, lokacija) i listom datuma održavanja.
/// </summary>
public class Concert
{
    /// <summary>Jedinstveni identifikator koncerta.</summary>
    public Guid Id { get; set; }
    /// <summary>Naziv koncerta (npr. "Eros Ramazzotti — Tour 2026").</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Grad u kojem se koncert održava.</summary>
    public string City { get; set; } = string.Empty;
    /// <summary>Naziv lokacije (dvorane, stadiona) gde se koncert održava.</summary>
    public string Venue { get; set; } = string.Empty;
    /// <summary>Adresa lokacije održavanja koncerta.</summary>
    public string Address { get; set; } = string.Empty;
    /// <summary>Navigation property ka listi datuma održavanja koncerta.</summary>
    public ICollection<ConcertDate> Dates { get; set; } = new List<ConcertDate>();
    /// <summary>Dodatne informacije o koncertu (organizatori, sponzori, napomene).</summary>
    public string AdditionalInfo { get; set; } = string.Empty;
}
