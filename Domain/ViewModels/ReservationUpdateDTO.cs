using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.ViewModels;

public class ReservationUpdateDTO
{
    [Required]
    public string CustomerEmail { get; set; } = string.Empty;
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    [MinLength(1, ErrorMessage = "At least one ticket must be requested.")]
    public List<TicketRequest> Tickets { get; set; } = [];
   
}
