using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> _logger;
        IHomeService _homeService;
        public HomeController(ILogger<HomeController> logger,IHomeService homeService)
        {
            logger = _logger ?? throw new ArgumentNullException(nameof(logger));
            _homeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
        }
        public IActionResult Index()
        {
            try
            {
                var homePage = _homeService.GetHomePage();
                return View(homePage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching home page.");
                ViewBag.ErrorMessage = "An error occurred while loading the page. Please try again later.";
                return View();
            }
        }
    }
}
