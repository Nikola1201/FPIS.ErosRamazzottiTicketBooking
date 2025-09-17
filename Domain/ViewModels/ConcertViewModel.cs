namespace FPIS.Domain.ViewModels;

public class ConcertViewModel
{
    public string? Name { get; set; }
    public string? City { get; set; }
    public string? Venue { get; set; }
    public string? Address { get; set; }
    public List<DateTime>? Dates { get; set; }
    public string? AdditionalInfo { get; set; }
}