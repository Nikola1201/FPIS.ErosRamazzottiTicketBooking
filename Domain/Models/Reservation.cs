namespace Domain.Models;

public class Reservation
{
    public int Id { get; set; }
    public Customer Customer { get; set; } = default!;
    public Token Token { get; set; } = default!;
    public PromoCode? PromoCode { get; set; }
    public ReservationStatus Status { get; set; }
    public List<ReservationTicket> Tickets { get; set; } = new();
}
