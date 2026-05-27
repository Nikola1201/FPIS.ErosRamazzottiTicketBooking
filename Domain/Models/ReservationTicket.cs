namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja jednu kartu u okviru rezervacije; vezuje se za zonu i datum koncerta.
/// </summary>
public class ReservationTicket
{
    /// <summary>Jedinstveni identifikator karte.</summary>
    public Guid Id { get; set; }
    /// <summary>Strani ključ ka <see cref="Reservation"/> kojoj karta pripada.</summary>
    public Guid ReservationId { get; set; }
    /// <summary>Strani ključ ka <see cref="Models.Zone"/> u kojoj se karta nalazi.</summary>
    public Guid ZoneId { get; set; }
    /// <summary>Navigation property ka <see cref="Models.Zone"/>.</summary>
    public Zone Zone { get; set; } = default!;
    /// <summary>Cena karte u trenutku rezervacije (zamrznuta vrednost).</summary>
    public decimal Price { get; set; }
    /// <summary>Strani ključ ka <see cref="ConcertDate"/> za koji karta važi.</summary>
    public Guid ConcertDateId { get; set; }
}
