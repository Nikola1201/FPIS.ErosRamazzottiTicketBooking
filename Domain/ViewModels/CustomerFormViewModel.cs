namespace FPIS.Domain.ViewModels;

/// <summary>
/// View model sa labelama polja forme za unos podataka o kupcu; koristi se za lokalizovani prikaz na klijentu.
/// </summary>
public class CustomerFormViewModel
{
    /// <summary>Labela za polje ime.</summary>
    public string FirstNameLabel { get; set; } = "First Name";
    /// <summary>Labela za polje prezime.</summary>
    public string LastNameLabel { get; set; } = "Last Name";
    /// <summary>Labela za polje email.</summary>
    public string EmailLabel { get; set; } = "Email";
    /// <summary>Labela za polje broj telefona.</summary>
    public string PhoneNumberLabel { get; set; } = "Phone Number";
    /// <summary>Labela za polje primarna adresa.</summary>
    public string AddressLabel { get; set; } = "Address";
    /// <summary>Labela za polje sekundarna adresa.</summary>
    public string Address2Label { get; set; } = "Address 2";
    /// <summary>Labela za polje grad.</summary>
    public string CityLabel { get; set; } = "City";
    /// <summary>Labela za polje poštanski broj.</summary>
    public string PostalCodeLabel { get; set; } = "Postal Code";
    /// <summary>Labela za polje država.</summary>
    public string CountryLabel { get; set; } = "Country";
    /// <summary>Labela za polje kompanija.</summary>
    public string CompanyLabel { get; set; } = "Company";
    /// <summary>Labela za polje potvrda email-a.</summary>
    public string ConfirmEmailLabel { get; set; } = "Confirm Email";
}
