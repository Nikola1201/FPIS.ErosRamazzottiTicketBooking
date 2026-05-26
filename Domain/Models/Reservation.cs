namespace FPIS.Domain.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public Customer Customer { get; set; } = default!;
    public AccessToken AccessToken { get; set; } = default!;
    public PromoCode? UsedPromoCode { get; set; }
    public Guid? UsedPromoCodeId { get; set; }
    public PromoCode GeneratedPromoCode { get; set; } = default!;
    public Guid? GeneratedPromoCodeId { get; set; }
    public ReservationStatus Status { get; set; }
    public ICollection<ReservationTicket> Tickets { get; set; } = [];
    public ICollection<Discount> Discounts { get; set; } = [];
}
