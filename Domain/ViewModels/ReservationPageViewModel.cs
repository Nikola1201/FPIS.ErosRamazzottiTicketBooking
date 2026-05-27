
namespace FPIS.Domain.ViewModels;

/// <summary>
/// View model za stranicu rezervacije; sadrži tekstove, formu kupca, podatke o koncertu i raspoloživim zonama po datumima.
/// </summary>
public class ReservationPageViewModel
{
    /// <summary>Naslov stranice rezervacije.</summary>
    public string? Title { get; set; } = "Eros Ramazzotti Live in Concert!";
    /// <summary>Podnaslov stranice rezervacije.</summary>
    public string? Subtitle { get; set; } = "Book your tickets now for an unforgettable experience.";
    /// <summary>URL hero slike na stranici rezervacije.</summary>
    public string? ImageUrl { get; set; } = "https://erosramazzottitour2026.com/assets/images/gallery/4.jpg";
    /// <summary>Opisni tekst stranice rezervacije.</summary>
    public string? Description { get; set; } = "Join us for an unforgettable evening with Eros Ramazzotti. Secure your seats now!";
    /// <summary>Forma za unos podataka o kupcu (<see cref="CustomerFormViewModel"/>).</summary>
    public CustomerFormViewModel CustomerForm { get; set; } = new();
    /// <summary>Informacije o koncertu (<see cref="ConcertViewModel"/>).</summary>
    public ConcertViewModel? Concert { get; set; }
    /// <summary>Lista raspoloživih datuma koncerta sa pripadajućim zonama.</summary>
    public List<ConcertDateViewModel> Dates { get; set; } = [];
    /// <summary>Konfiguracioni parametri aplikacije relevantni za stranicu (key/value parovi).</summary>
    public Dictionary<string, string> AppSettings { get; set; } = new();

}

/// <summary>
/// View model za jedan datum koncerta sa listom raspoloživih zona.
/// </summary>
public class ConcertDateViewModel
{
    /// <summary>Identifikator datuma koncerta.</summary>
    public Guid Id { get; set; }
    /// <summary>Datum i vreme održavanja koncerta.</summary>
    public DateTime Date { get; set; }
    /// <summary>Lista raspoloživih zona za ovaj datum.</summary>
    public List<ZoneViewModel> Zones { get; set; } = new();
}

/// <summary>
/// View model zone na stranici rezervacije sa kapacitetom i preostalim mestima.
/// </summary>
public class ZoneViewModel
{
    /// <summary>Identifikator zone.</summary>
    public Guid Id { get; set; }
    /// <summary>Naziv zone.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Ukupan kapacitet zone.</summary>
    public int Capacity { get; set; }
    /// <summary>Broj preostalih (slobodnih) mesta u zoni.</summary>
    public int CapacityRemaining { get; set; }
    /// <summary>Cena karte u zoni.</summary>
    public decimal Price { get; set; }
}
