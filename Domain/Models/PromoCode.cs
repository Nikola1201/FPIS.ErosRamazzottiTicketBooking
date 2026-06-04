namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja promo kod koji se može iskoristiti za popust; vezan je za rezervaciju koja ga je generisala i/ili iskoristila.
/// </summary>
public class PromoCode
{
    /// <summary>Jedinstveni identifikator promo koda.</summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Tekstualna vrednost promo koda.
    /// Dozvoljene vrednosti: obavezno, tačno 10 alfanumeričkih znakova (A–Z, a–z, 0–9).
    /// </summary>
    public string Code { get; set; } = default!;
    /// <summary>Da li je promo kod već iskorišćen.</summary>
    public bool IsUsed { get; set; }
    /// <summary>Da li je promo kod aktivan (može se koristiti).</summary>
    public bool IsActive { get; set; }
    /// <summary>Strani ključ ka rezervaciji koja je iskoristila promo kod (opciono).</summary>
    public Guid? UsedByReservationId { get; set; }
    /// <summary>Navigation property ka rezervaciji koja je iskoristila promo kod (opciono).</summary>
    public Reservation? UsedByReservation { get; set; }
    /// <summary>Strani ključ ka rezervaciji koja je generisala promo kod (opciono).</summary>
    public Guid? GeneratedByReservationId { get; set; }
    /// <summary>Navigation property ka rezervaciji koja je generisala promo kod (opciono).</summary>
    public Reservation? GeneratedByReservation { get; set; }
}
