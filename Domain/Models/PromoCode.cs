namespace FPIS.Domain.Models;

public class PromoCode
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public bool IsUsed { get; set; }
    public bool IsActive { get; set; }
    public Guid? UsedByReservationId { get; set; }
    public Reservation? UsedByReservation { get; set; }
    public Guid? GeneratedByReservationId { get; set; }
    public Reservation? GeneratedByReservation { get; set; }
}
