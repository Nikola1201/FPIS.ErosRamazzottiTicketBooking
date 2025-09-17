namespace FPIS.Domain.Models;

public class Concert
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<ConcertDate> Dates { get; set; } = new List<ConcertDate>();
    public string AdditionalInfo { get; set; } = string.Empty;
}
