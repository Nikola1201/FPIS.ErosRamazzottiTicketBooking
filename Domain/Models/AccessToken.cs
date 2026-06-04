namespace FPIS.Domain.Models;

/// <summary>
/// Token za pristup rezervaciji; služi kao tajni identifikator koji se šalje kupcu da bi mogao da pregleda ili izmeni rezervaciju.
/// </summary>
public class AccessToken
{
    /// <summary>Jedinstveni identifikator tokena.</summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Strani ključ ka <see cref="Models.Reservation"/> kojoj token pripada.
    /// Dozvoljene vrednosti: obavezno, mora biti različito od Guid.Empty.
    /// </summary>
    public Guid ReservationId { get; set; }
    /// <summary>Navigation property ka <see cref="Models.Reservation"/>.</summary>
    public Reservation Reservation { get; set; } = default!;
    /// <summary>
    /// Tekstualna vrednost tokena.
    /// Dozvoljene vrednosti: obavezno, dužina 1–256 znakova.
    /// </summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>Da li je token aktivan (može se koristiti za pristup rezervaciji).</summary>
    public bool IsActive { get; set; }
}


