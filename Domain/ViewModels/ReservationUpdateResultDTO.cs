namespace FPIS.Domain.ViewModels;

public class ReservationUpdateResultDTO
{

    public Guid ReservationId { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool Updated { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }
    public int UpdatedTicketCount { get; set; }
    public string Message { get; set; }
}
