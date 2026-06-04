using FPIS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace FPIS.Domain.Validation;

/// <summary>
/// Validator za <see cref="AppSettings"/>. Vraća listu grešaka; prazna lista znači da je model validan.
/// </summary>
public static class AppSettingsValidator
{
    /// <summary>
    /// Validira <see cref="AppSettings"/> i vraća listu grešaka.
    /// Prazna lista znači da je model validan.
    /// </summary>
    /// <param name="settings">Konfiguraciona stavka koja se validira.</param>
    /// <returns>Lista <see cref="ValidationResult"/> grešaka; prazna ako je model validan.</returns>
    /// <exception cref="ArgumentNullException">Baca se ako je <paramref name="settings"/> <c>null</c>.</exception>
    public static IReadOnlyList<ValidationResult> Validate(AppSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        var errors = new List<ValidationResult>();

        if (string.IsNullOrWhiteSpace(settings.Key) || settings.Key.Length > 100)
            errors.Add(new ValidationResult(
                "Key je obavezan, dužina 1–100 znakova.",
                new[] { nameof(AppSettings.Key) }));

        if (string.IsNullOrWhiteSpace(settings.Value) || settings.Value.Length > 2000)
            errors.Add(new ValidationResult(
                "Value je obavezan, dužina 1–2000 znakova.",
                new[] { nameof(AppSettings.Value) }));

        return errors;
    }
}
