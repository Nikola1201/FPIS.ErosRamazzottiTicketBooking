using FPIS.Domain.Guards;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.ViewModels;

/// <summary>
/// DTO za kreiranje nove rezervacije; sadrži podatke o kupcu, izabrani datum koncerta, traženim kartama i opcionim promo kodom.
/// </summary>
public class ReservationPostDTO
{
    /// <summary>Podaci o kupcu (vidi <see cref="CustomerCreateDTO"/>).</summary>
    [Required]
    public CustomerCreateDTO Customer { get; set; } = new();

    /// <summary>Identifikator datuma koncerta za koji se vrši rezervacija.</summary>
    [Required]
    public Guid ConcertDateId { get; set; }

    /// <summary>Lista zahteva za karte po zonama.</summary>
    [Required]
    [MinLength(1, ErrorMessage = "At least one ticket must be requested.")]
    public List<TicketRequest> Tickets { get; set; } = [];
    /// <summary>Opcioni promo kod (tačno 10 karaktera).</summary>
    [StringLength(10, MinimumLength = 10, ErrorMessage = "PromoCode must be exactly 10 characters long.")]
    public string? PromoCode { get; set; }
}

/// <summary>
/// DTO sa podacima o kupcu pri kreiranju rezervacije; validacija nameće da se Email i ConfirmedEmail poklapaju.
/// </summary>
[EmailMatch]
public class CustomerCreateDTO
{
    /// <summary>Ime kupca.</summary>
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Prezime kupca.</summary>
    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>Email adresa kupca.</summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    /// <summary>Potvrda email adrese (mora se poklapati sa <see cref="Email"/>).</summary>
    [Required, EmailAddress]
    public string ConfirmedEmail { get; set; } = string.Empty;

    /// <summary>Broj telefona kupca.</summary>
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>Primarna adresa kupca.</summary>
    [Required, StringLength(200)]
    public string Address { get; set; } = string.Empty;

    /// <summary>Opciona sekundarna adresa.</summary>
    [StringLength(200)]
    public string Address2 { get; set; } = string.Empty;

    /// <summary>Grad kupca.</summary>
    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    /// <summary>Poštanski broj.</summary>
    [Required, StringLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>Država kupca.</summary>
    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;

    /// <summary>Opcioni naziv kompanije.</summary>
    [StringLength(100)]
    public string Company { get; set; } = string.Empty;

}

/// <summary>
/// Zahtev za karte u određenoj zoni; deo <see cref="ReservationPostDTO"/>.
/// </summary>
public class TicketRequest
{
    /// <summary>Identifikator zone za koju se traže karte.</summary>
    [Required]
    public Guid ZoneId { get; set; }

    /// <summary>Broj traženih karata u zoni (1–100).</summary>
    [Range(1, 100, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
