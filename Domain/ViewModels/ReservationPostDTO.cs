using FPIS.Domain.Guards;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.ViewModels;

public class ReservationPostDTO
{
    [Required]
    public CustomerCreateDTO Customer { get; set; } = new();

    [Required]
    public Guid ConcertDateId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one ticket must be requested.")]
    public List<TicketRequest> Tickets { get; set; } = [];
    [StringLength(10, MinimumLength = 10, ErrorMessage = "PromoCode must be exactly 10 characters long.")]
    public string? PromoCode { get; set; }
}

[EmailMatch]
public class CustomerCreateDTO
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string ConfirmedEmail { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [StringLength(200)]
    public string Address2 { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [StringLength(100)]
    public string Company { get; set; } = string.Empty;
 
}

public class TicketRequest
{
    [Required]
    public Guid ZoneId { get; set; }

    [Range(1, 100, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
