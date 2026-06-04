using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="PromoCode"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class PromoCodeValidator
{
    private static readonly Regex CodeRegex =
        new(@"^[A-Za-z0-9]{10}$", RegexOptions.Compiled);

    /// <summary>
    /// Validira <see cref="PromoCode"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="promoCode">Promo kod koji se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="promoCode"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(PromoCode promoCode)
    {
        ArgumentNullException.ThrowIfNull(promoCode);
        var errors = new List<ValidationResult>();

        if (promoCode.Code is null || !CodeRegex.IsMatch(promoCode.Code))
            errors.Add(new ValidationResult(
                "Code je obavezan i mora imati tačno 10 alfanumeričkih znakova.",
                new[] { nameof(PromoCode.Code) }));

        return errors;
    }
}
