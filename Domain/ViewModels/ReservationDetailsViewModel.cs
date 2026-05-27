using FPIS.Domain.Models;

namespace FPIS.Domain.ViewModels;

/// <summary>
/// Detaljan view model rezervacije: status, kupac, karte, popusti, promo kodovi i informacije o koncertu.
/// </summary>
public class ReservationDetailsViewModel
{
    /// <summary>Identifikator rezervacije.</summary>
    public Guid ReservationId { get; set; }
    /// <summary>Status rezervacije kao tekst.</summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>Puno ime kupca (ime i prezime).</summary>
    public string CustomerName { get; set; } = string.Empty;
    /// <summary>Email adresa kupca.</summary>
    public string CustomerEmail { get; set; } = string.Empty;
    /// <summary>Pristupni token rezervacije.</summary>
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>Promo kod koji je rezervacija iskoristila (opciono).</summary>
    public string? UsedPromoCode { get; set; }
    /// <summary>Promo kod koji je rezervacija generisala (opciono).</summary>
    public string? GeneratedPromoCode { get; set; }
    /// <summary>Lista detalja karata u rezervaciji.</summary>
    public List<TicketDetailsViewModel> Tickets { get; set; } = [];
    /// <summary>Lista popusta primenjenih na rezervaciju.</summary>
    public List<DiscountDetailsViewModel> Discounts { get; set; } = [];
    /// <summary>Datum i vreme koncerta za koji važi rezervacija.</summary>
    public DateTime? ConcertDate { get; set; }
    /// <summary>Naziv koncerta.</summary>
    public string? ConcertName { get; set; }
    /// <summary>Naziv lokacije održavanja koncerta.</summary>
    public string? ConcertVenue { get; set; }
    /// <summary>Grad u kojem se koncert održava.</summary>
    public string? ConcertCity { get; set; }
    /// <summary>Ukupna cena karata pre primene popusta.</summary>
    public decimal TotalPrice { get; set; }
    /// <summary>Konačna cena rezervacije nakon primene popusta.</summary>
    public decimal FinalPrice { get; set; }
    /// <summary>Lista detalja zona vezanih za karte u rezervaciji.</summary>
    public List<ZoneViewModel>? ZonesDetails { get; set; }
    /// <summary>Da li je generisani promo kod već iskorišćen.</summary>
    public bool IsGeneratedPromoCodeUsed { get; set; }
}

/// <summary>
/// Detalji jedne karte u rezervaciji: zona i cena.
/// </summary>
public class TicketDetailsViewModel
{
    /// <summary>Identifikator karte.</summary>
    public Guid TicketId { get; set; }
    /// <summary>Naziv zone u kojoj se karta nalazi.</summary>
    public string ZoneName { get; set; } = string.Empty;
    /// <summary>Cena karte u trenutku rezervacije.</summary>
    public decimal Price { get; set; }
}

/// <summary>
/// Detalji jednog popusta primenjenog na rezervaciju.
/// </summary>
public class DiscountDetailsViewModel
{
    /// <summary>Tip popusta kao tekst.</summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>Procenat popusta.</summary>
    public decimal Percentage { get; set; }
}
