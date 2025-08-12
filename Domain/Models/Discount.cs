namespace Domain.Models;

public class Discount
{
    public int Id { get; set; }
    public DiscountType Type { get; set; }
    public decimal Percentage { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
}
