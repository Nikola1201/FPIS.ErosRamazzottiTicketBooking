namespace Domain.Models;

public class Token
{
    public string Value { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime ExpiryDate { get; set; }
}
