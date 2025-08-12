namespace Domain.Models;

public class PromoCode
{
    public string Code { get; set; } = default!;
    public bool IsUsed { get; set; }
    public int? LinkedReservationId { get; set; }
}
