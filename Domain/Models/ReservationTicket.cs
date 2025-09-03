namespace FPIS.Domain.Models;

public class ReservationTicket
{
    public Guid Id { get; set; }
    public Guid ReservationId { get; set; }
    public Guid ZoneId { get; set; }
    public Zone Zone { get; set; } = default!;
    public decimal Price { get; set; }
}
