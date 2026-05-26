namespace FPIS.Domain.Models;
public class Zone
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal Price { get; set; }
}
