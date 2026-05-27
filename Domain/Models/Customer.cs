namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja kupca koji vrši rezervaciju karata; sadrži lične i kontakt podatke.
/// </summary>
public class Customer
{
    /// <summary>Jedinstveni identifikator kupca.</summary>
    public Guid Id { get; set; }
    /// <summary>Ime kupca.</summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>Prezime kupca.</summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>Email adresa kupca.</summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>Potvrda email adrese (mora se poklapati sa <see cref="Email"/>).</summary>
    public string ConfirmedEmail { get; set; } = string.Empty;
    /// <summary>Primarna adresa kupca.</summary>
    public string Address { get; set; } = string.Empty;
    /// <summary>Opciona sekundarna adresa (npr. sprat ili stan).</summary>
    public string? Address2 { get; set; }
    /// <summary>Grad u kojem kupac živi.</summary>
    public string City { get; set; } = string.Empty;
    /// <summary>Poštanski broj.</summary>
    public string PostalCode { get; set; } = string.Empty;
    /// <summary>Država kupca.</summary>
    public string Country { get; set; } = string.Empty;
    /// <summary>Opcioni broj telefona kupca.</summary>
    public string? PhoneNumber { get; set; }
    /// <summary>Opcioni naziv kompanije (ako kupac kupuje u ime firme).</summary>
    public string? Company { get; set; }
}
