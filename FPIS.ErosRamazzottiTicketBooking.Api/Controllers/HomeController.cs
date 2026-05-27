using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;

/// <summary>
/// API kontroler za home page — vraća osnovne informacije o trenutnom koncertu i CTA elemente.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly IHomeService _homeService;
    private readonly ILogger<HomeController> _logger;

    /// <summary>Konstruktor sa injektovanim servisom i logger-om.</summary>
    /// <param name="homeService">Servis koji obezbeđuje podatke za home page.</param>
    /// <param name="logger">Logger za greške i upozorenja.</param>
    public HomeController(IHomeService homeService, ILogger<HomeController> logger)
    {
        _homeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    /// <summary>GET /api/home — vraća <see cref="FPIS.Domain.ViewModels.HomePageViewModel"/> sa podacima o aktuelnom koncertu.</summary>
    /// <returns>200 OK sa view modelom ili 404/500 sa <see cref="ProblemDetails"/>.</returns>
    /// <response code="200">Uspešno vraćen home page view model.</response>
    /// <response code="404">Nije pronađen koncert.</response>
    /// <response code="500">Interna greška servera.</response>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _homeService.GetHomePage();
        if (result.IsSuccess)
            return Ok(result.Value);

        _logger.LogWarning("Home page error: {Error}", result.Error);
        if (result.ErrorCode == 404)
            return NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Status = 404,
                Detail = result.Error
            });

        return StatusCode(result.ErrorCode ?? 500, new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = result.ErrorCode ?? 500,
            Detail = result.Error
        });
    }

}
