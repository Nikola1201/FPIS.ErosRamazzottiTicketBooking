namespace FPIS.Domain.Models;

public class Token
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime ExpiryDate { get; set; }
}
