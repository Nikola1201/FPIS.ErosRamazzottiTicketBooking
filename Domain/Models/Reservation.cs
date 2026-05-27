namespace FPIS.Domain.Models;

/// <summary>
/// Agregat rezervacije: povezuje kupca, karte, popuste, promo kodove i status rezervacije.
/// </summary>
public class Reservation
{
    /// <summary>Jedinstveni identifikator rezervacije.</summary>
    public Guid Id { get; set; }
    /// <summary>Navigation property ka kupcu (<see cref="Models.Customer"/>) koji je napravio rezervaciju.</summary>
    public Customer Customer { get; set; } = default!;
    /// <summary>Navigation property ka <see cref="Models.AccessToken"/> koji se koristi za pristup rezervaciji.</summary>
    public AccessToken AccessToken { get; set; } = default!;
    /// <summary>Navigation property ka iskorišćenom promo kodu (opciono).</summary>
    public PromoCode? UsedPromoCode { get; set; }
    /// <summary>Strani ključ ka iskorišćenom <see cref="PromoCode"/> (opciono).</summary>
    public Guid? UsedPromoCodeId { get; set; }
    /// <summary>Navigation property ka generisanom promo kodu koji rezervacija dobija kao nagradu.</summary>
    public PromoCode GeneratedPromoCode { get; set; } = default!;
    /// <summary>Strani ključ ka generisanom <see cref="PromoCode"/> (opciono).</summary>
    public Guid? GeneratedPromoCodeId { get; set; }
    /// <summary>Trenutni status rezervacije (aktivna, izmenjena, otkazana).</summary>
    public ReservationStatus Status { get; set; }
    /// <summary>Navigation property ka kartama (<see cref="ReservationTicket"/>) u rezervaciji.</summary>
    public ICollection<ReservationTicket> Tickets { get; set; } = [];
    /// <summary>Navigation property ka popustima (<see cref="Discount"/>) primenjenim na rezervaciju.</summary>
    public ICollection<Discount> Discounts { get; set; } = [];
}
