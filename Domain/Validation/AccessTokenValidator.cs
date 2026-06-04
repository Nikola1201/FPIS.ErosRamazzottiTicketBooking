using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="AccessToken"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class AccessTokenValidator
{
    /// <summary>
    /// Validira <see cref="AccessToken"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="token">Token koji se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="token"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(AccessToken token)
    {
        ArgumentNullException.ThrowIfNull(token);
        var errors = new List<ValidationResult>();

        if (token.ReservationId == Guid.Empty)
            errors.Add(new ValidationResult(
                "ReservationId je obavezan i ne sme biti Guid.Empty.",
                new[] { nameof(AccessToken.ReservationId) }));

        if (string.IsNullOrWhiteSpace(token.Value) || token.Value.Length > 256)
            errors.Add(new ValidationResult(
                "Value je obavezan, dužina 1–256 znakova.",
                new[] { nameof(AccessToken.Value) }));

        return errors;
    }
}
