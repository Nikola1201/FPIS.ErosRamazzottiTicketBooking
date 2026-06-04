using System.Net.Sockets;

namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja konkretan datum održavanja koncerta sa pripadajućim kartama.
/// </summary>
public class ConcertDate
{
    /// <summary>Jedinstveni identifikator datuma koncerta.</summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Datum i vreme održavanja koncerta.
    /// Dozvoljene vrednosti: obavezno, mora biti različito od default(DateTime).
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// Strani ključ ka <see cref="Models.Concert"/>.
    /// Dozvoljene vrednosti: obavezno, mora biti različito od Guid.Empty.
    /// </summary>
    public Guid ConcertId { get; set; }
    /// <summary>Navigation property ka <see cref="Models.Concert"/>.</summary>
    public Concert Concert { get; set; } = new();
    /// <summary>Navigation property ka kartama (<see cref="ReservationTicket"/>) izdatim za ovaj datum.</summary>
    public ICollection<ReservationTicket> Tickets { get; set; } = [];
}
