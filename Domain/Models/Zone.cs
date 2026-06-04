namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja zonu (kategoriju mesta) na koncertu sa kapacitetom i cenom karte.
/// </summary>
public class Zone
{
    /// <summary>Jedinstveni identifikator zone.</summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Naziv zone (npr. "VIP", "Parter", "Tribina").
    /// Dozvoljene vrednosti: obavezno, dužina 1–100 znakova.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Ukupan kapacitet zone (maksimalan broj karata).
    /// Dozvoljene vrednosti: veće od 0.
    /// </summary>
    public int Capacity { get; set; }
    /// <summary>
    /// Cena jedne karte u datoj zoni.
    /// Dozvoljene vrednosti: ≥ 0.
    /// </summary>
    public decimal Price { get; set; }
}
