namespace FPIS.Domain.Models;

/// <summary>
/// Predstavlja kupca koji vrši rezervaciju karata; sadrži lične i kontakt podatke.
/// </summary>
public class Customer
{
    /// <summary>Jedinstveni identifikator kupca.</summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Ime kupca.
    /// Dozvoljene vrednosti: obavezno, dužina 1–100 znakova.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// Prezime kupca.
    /// Dozvoljene vrednosti: obavezno, dužina 1–100 znakova.
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// Email adresa kupca.
    /// Dozvoljene vrednosti: obavezno, dužina ≤ 254 znaka, format lokal@domen.tld.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Potvrda email adrese (mora se poklapati sa <see cref="Email"/>).
    /// Dozvoljene vrednosti: obavezno, mora biti identičan sa Email.
    /// </summary>
    public string ConfirmedEmail { get; set; } = string.Empty;
    /// <summary>
    /// Primarna adresa kupca.
    /// Dozvoljene vrednosti: obavezno, dužina 1–200 znakova.
    /// </summary>
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// Opciona sekundarna adresa (npr. sprat ili stan).
    /// Dozvoljene vrednosti: null ili 1–200 znakova.
    /// </summary>
    public string? Address2 { get; set; }
    /// <summary>
    /// Grad u kojem kupac živi.
    /// Dozvoljene vrednosti: obavezno, dužina 1–100 znakova.
    /// </summary>
    public string City { get; set; } = string.Empty;
    /// <summary>
    /// Poštanski broj.
    /// Dozvoljene vrednosti: obavezno, dužina 1–20 znakova.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;
    /// <summary>
    /// Država kupca.
    /// Dozvoljene vrednosti: obavezno, dužina 1–100 znakova.
    /// </summary>
    public string Country { get; set; } = string.Empty;
    /// <summary>
    /// Opcioni broj telefona kupca.
    /// Dozvoljene vrednosti: null ili 3–30 znakova (cifre, razmak, + ili -).
    /// </summary>
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// Opcioni naziv kompanije (ako kupac kupuje u ime firme).
    /// Dozvoljene vrednosti: null ili 1–100 znakova.
    /// </summary>
    public string? Company { get; set; }
}
