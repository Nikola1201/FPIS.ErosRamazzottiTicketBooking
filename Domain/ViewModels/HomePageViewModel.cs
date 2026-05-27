namespace FPIS.Domain.ViewModels;

/// <summary>
/// View model za prikaz home page-a aplikacije sa osnovnim informacijama o trenutnom koncertu i CTA elementima.
/// </summary>
public class HomePageViewModel
{
    /// <summary>Naslov koji se prikazuje na home page-u.</summary>
    public string? Title { get; set; }
    /// <summary>Podnaslov ispod glavnog naslova.</summary>
    public string? Subtitle { get; set; }
    /// <summary>URL slike za pozadinu ili banner.</summary>
    public string? ImageUrl { get; set; }
    /// <summary>Opisni tekst na home page-u.</summary>
    public string? Description { get; set; }
    /// <summary>Tekst koji se prikazuje na CTA dugmetu.</summary>
    public string? ButtonText { get; set; }
    /// <summary>URL na koji vodi CTA dugme.</summary>
    public string? ButtonUrl { get; set; }
    /// <summary>Informacije o aktuelnom koncertu (<see cref="ConcertViewModel"/>).</summary>
    public ConcertViewModel? Concert { get; set; }
}
