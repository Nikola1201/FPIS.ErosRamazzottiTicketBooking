
namespace FPIS.Domain.Models;

/// <summary>
/// Konfiguraciona stavka aplikacije sačuvana u bazi (key/value par).
/// </summary>
public class AppSettings
{
    /// <summary>Jedinstveni identifikator stavke.</summary>
    public Guid Id { get; set; }
    /// <summary>Ključ konfiguracione stavke.</summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>Vrednost konfiguracione stavke.</summary>
    public string Value { get; set; } = string.Empty;
}
