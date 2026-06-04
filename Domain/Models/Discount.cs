namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja popust primenjen na rezervaciju; definiše tip i procenat popusta.
/// </summary>
public class Discount
{
    /// <summary>Jedinstveni identifikator popusta.</summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Tip popusta (vidi <see cref="DiscountType"/>).
    /// Dozvoljene vrednosti: definisana vrednost iz <see cref="DiscountType"/>.
    /// </summary>
    public DiscountType Type { get; set; }
    /// <summary>
    /// Procenat popusta (npr. 10 znači 10%).
    /// Dozvoljene vrednosti: decimalna vrednost u opsegu 0–100 (uključivo).
    /// </summary>
    public decimal Percentage { get; set; }
    /// <summary>
    /// Strani ključ ka <see cref="Models.Reservation"/> na koju se popust odnosi.
    /// Dozvoljene vrednosti: obavezno, mora biti različito od Guid.Empty.
    /// </summary>
    public Guid ReservationId { get; set; }
    /// <summary>Navigation property ka <see cref="Models.Reservation"/>.</summary>
    public Reservation Reservation { get; set; } = default!;
}
