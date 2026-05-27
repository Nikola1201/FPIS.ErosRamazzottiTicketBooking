namespace FPIS.Domain.ViewModels;

/// <summary>
/// DTO koji se vraća klijentu nakon uspešne rezervacije; sadrži identifikator rezervacije i pristupni token.
/// </summary>
public class ReservationResultDTO
{
    /// <summary>Identifikator novokreirane rezervacije.</summary>
    public Guid ReservationId { get; set; }
    /// <summary>Pristupni token za rezervaciju (šalje se kupcu).</summary>
    public string Token { get; set; } = string.Empty;
}
