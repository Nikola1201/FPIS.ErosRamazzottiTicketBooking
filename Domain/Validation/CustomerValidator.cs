using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="Customer"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class CustomerValidator
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    private static readonly Regex PhoneRegex =
        new(@"^[+\-\d\s]{3,30}$", RegexOptions.Compiled);

    /// <summary>
    /// Validira <see cref="Customer"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="customer">Objekat kupca koji se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="customer"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        var errors = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(customer.FirstName) || customer.FirstName.Length > 100)
            errors.Add(new ValidationResult(
                "FirstName je obavezan, dužina 1–100 znakova.",
                new[] { nameof(Customer.FirstName) }));

        if (string.IsNullOrWhiteSpace(customer.LastName) || customer.LastName.Length > 100)
            errors.Add(new ValidationResult(
                "LastName je obavezan, dužina 1–100 znakova.",
                new[] { nameof(Customer.LastName) }));

        if (string.IsNullOrWhiteSpace(customer.Email)
            || customer.Email.Length > 254
            || !EmailRegex.IsMatch(customer.Email))
            errors.Add(new ValidationResult(
                "Email je obavezan i mora biti u ispravnom formatu (≤ 254 znaka).",
                new[] { nameof(Customer.Email) }));

        if (customer.Email != customer.ConfirmedEmail)
            errors.Add(new ValidationResult(
                "ConfirmedEmail mora biti identičan sa Email.",
                new[] { nameof(Customer.ConfirmedEmail) }));

        if (string.IsNullOrWhiteSpace(customer.Address) || customer.Address.Length > 200)
            errors.Add(new ValidationResult(
                "Address je obavezan, dužina 1–200 znakova.",
                new[] { nameof(Customer.Address) }));

        if (customer.Address2 is not null
            && (string.IsNullOrWhiteSpace(customer.Address2) || customer.Address2.Length > 200))
            errors.Add(new ValidationResult(
                "Address2, ako je zadat, mora biti dužine 1–200 znakova.",
                new[] { nameof(Customer.Address2) }));

        if (string.IsNullOrWhiteSpace(customer.City) || customer.City.Length > 100)
            errors.Add(new ValidationResult(
                "City je obavezan, dužina 1–100 znakova.",
                new[] { nameof(Customer.City) }));

        if (string.IsNullOrWhiteSpace(customer.PostalCode) || customer.PostalCode.Length > 20)
            errors.Add(new ValidationResult(
                "PostalCode je obavezan, dužina 1–20 znakova.",
                new[] { nameof(Customer.PostalCode) }));

        if (string.IsNullOrWhiteSpace(customer.Country) || customer.Country.Length > 100)
            errors.Add(new ValidationResult(
                "Country je obavezan, dužina 1–100 znakova.",
                new[] { nameof(Customer.Country) }));

        if (customer.PhoneNumber is not null
            && (string.IsNullOrWhiteSpace(customer.PhoneNumber) || !PhoneRegex.IsMatch(customer.PhoneNumber)))
            errors.Add(new ValidationResult(
                "PhoneNumber, ako je zadat, mora sadržati 3–30 znakova (cifre, razmak, + ili -).",
                new[] { nameof(Customer.PhoneNumber) }));

        if (customer.Company is not null
            && (string.IsNullOrWhiteSpace(customer.Company) || customer.Company.Length > 100))
            errors.Add(new ValidationResult(
                "Company, ako je zadat, mora biti dužine 1–100 znakova.",
                new[] { nameof(Customer.Company) }));

        return errors;
    }
}
