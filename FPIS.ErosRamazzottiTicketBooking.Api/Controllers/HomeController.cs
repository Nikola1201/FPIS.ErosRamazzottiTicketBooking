using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly IHomeService _homeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHomeService homeService, ILogger<HomeController> logger)
    {
        _homeService = homeService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var homePage = await _homeService.GetHomePage();
            if (homePage == null)
            {
                _logger.LogWarning("Home page data not found.");
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "Home page data could not be found."
                });
            }
            return Ok(homePage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching home page.");
            var problemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An error occurred while loading the page. Please try again later.",
                Instance = HttpContext.Request.Path
            };
            return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
        }
    }
}
