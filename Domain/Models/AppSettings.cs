
namespace FPIS.Domain.Models;

public class AppSettings
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
