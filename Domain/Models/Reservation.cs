namespace FPIS.Domain.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public Customer Customer { get; set; } = default!;
    public Token Token { get; set; } = default!;
    public PromoCode? PromoCode { get; set; }
    public ReservationStatus Status { get; set; }
    public ICollection<ReservationTicket> Tickets { get; set; } = [];
    public ICollection<Discount> Discounts { get; set; } = [];
}
