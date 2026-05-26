namespace FPIS.Domain.ViewModels;

public class ReservationCancelResultDTO
{
    public Guid ReservationId { get; set; }
    public bool Cancelled { get; set; }
    public string Message { get; set; } = string.Empty;
}
