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
        _homeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
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
