
namespace FPIS.Domain.ViewModels;

public class ReservationPageViewModel
{
    public string? Title { get; set; } = "Eros Ramazzotti Live in Concert!";
    public string? Subtitle { get; set; } = "Book your tickets now for an unforgettable experience.";
    public string? ImageUrl { get; set; } = "https://erosramazzottitour2026.com/assets/images/gallery/4.jpg";
    public string? Description { get; set; } = "Join us for an unforgettable evening with Eros Ramazzotti. Secure your seats now!";
    public CustomerFormViewModel CustomerForm { get; set; } = new();
    public ConcertViewModel? Concert { get; set; }
    public List<ConcertDateViewModel> Dates { get; set; } = [];
    public Dictionary<string, string> AppSettings { get; set; } = new();

}

public class ConcertDateViewModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<ZoneViewModel> Zones { get; set; } = new();
}

public class ZoneViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int CapacityRemaining { get; set; }
    public decimal Price { get; set; }
}