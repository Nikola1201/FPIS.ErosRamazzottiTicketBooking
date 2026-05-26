using FPIS.Domain.Models;

namespace FPIS.Domain.ViewModels;

public class ReservationDetailsViewModel
{
    public Guid ReservationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string? UsedPromoCode { get; set; }
    public string? GeneratedPromoCode { get; set; }
    public List<TicketDetailsViewModel> Tickets { get; set; } = [];
    public List<DiscountDetailsViewModel> Discounts { get; set; } = [];
    public DateTime? ConcertDate { get; set; }
    public string? ConcertName { get; set; }
    public string? ConcertVenue { get; set; }
    public string? ConcertCity { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal FinalPrice { get; set; }
    public List<ZoneViewModel>? ZonesDetails { get; set; }
    public bool IsGeneratedPromoCodeUsed { get; set; }
}

public class TicketDetailsViewModel
{
    public Guid TicketId { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class DiscountDetailsViewModel
{
    public string Type { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
}
