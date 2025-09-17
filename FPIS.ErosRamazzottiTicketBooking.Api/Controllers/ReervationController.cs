using Microsoft.AspNetCore.Mvc;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Controllers;

public class ReservationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
