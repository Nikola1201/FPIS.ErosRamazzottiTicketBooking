namespace FPIS.Domain.Models;

public class PromoCode
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public bool IsUsed { get; set; }
    public Guid? LinkedReservationId { get; set; }
}
