namespace FPIS.Domain.Models;

public class Discount
{
    public Guid Id { get; set; }
    public DiscountType Type { get; set; }
    public decimal Percentage { get; set; }
    public Guid ReservationId { get; set; }
    public Reservation Reservation { get; set; } = default!;
}
