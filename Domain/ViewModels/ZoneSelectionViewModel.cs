namespace FPIS.Domain.ViewModels;

public class ZoneSelectionViewModel
{
    public Guid ZoneId { get; set; }
    public string ZoneName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AvailableSeats { get; set; }
    public int SelectedSeats { get; set; }
    public int TotalSeats { get; set; }

}