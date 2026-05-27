using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.ViewModels;

/// <summary>
/// DTO za izmenu postojeće rezervacije; identifikuje rezervaciju email-om i pristupnim tokenom i nosi nove karte.
/// </summary>
public class ReservationUpdateDTO
{
    /// <summary>Email adresa kupca koji vrši izmenu (autorizacija).</summary>
    [Required]
    public string CustomerEmail { get; set; } = string.Empty;
    /// <summary>Pristupni token rezervacije (autorizacija).</summary>
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>Nova lista zahteva za karte po zonama.</summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one ticket must be requested.")]
    public List<TicketRequest> Tickets { get; set; } = [];

}
