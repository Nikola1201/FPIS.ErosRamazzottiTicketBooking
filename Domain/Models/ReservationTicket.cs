namespace Domain.Models;

public class ReservationTicket
{
    public int ReservationId { get; set; }
    public int ZoneId { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerTicket { get; set; }
}
