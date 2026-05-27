namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja popust primenjen na rezervaciju; definiše tip i procenat popusta.
/// </summary>
public class Discount
{
    /// <summary>Jedinstveni identifikator popusta.</summary>
    public Guid Id { get; set; }
    /// <summary>Tip popusta (vidi <see cref="DiscountType"/>).</summary>
    public DiscountType Type { get; set; }
    /// <summary>Procenat popusta (npr. 10 znači 10%).</summary>
    public decimal Percentage { get; set; }
    /// <summary>Strani ključ ka <see cref="Models.Reservation"/> na koju se popust odnosi.</summary>
    public Guid ReservationId { get; set; }
    /// <summary>Navigation property ka <see cref="Models.Reservation"/>.</summary>
    public Reservation Reservation { get; set; } = default!;
}
