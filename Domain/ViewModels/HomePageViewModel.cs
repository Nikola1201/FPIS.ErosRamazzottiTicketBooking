namespace FPIS.Domain.ViewModels;

public class HomePageViewModel
{
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public string? ButtonText { get; set; }
    public string? ButtonUrl { get; set; }
    public ConcertViewModel? Concert { get; set; }
}