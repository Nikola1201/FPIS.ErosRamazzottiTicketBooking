namespace FPIS.Domain.Models;

public class AccessToken
{
    public Guid Id { get; set; }
    public Guid ReservationId { get; set; }
    public Reservation Reservation { get; set; } = default!;
    public string Value { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}


