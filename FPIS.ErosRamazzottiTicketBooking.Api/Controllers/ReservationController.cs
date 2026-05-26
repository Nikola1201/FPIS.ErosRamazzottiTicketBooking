using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _reservationService.GetReservationPage();
        if (result.IsSuccess)
            return Ok(result.Value);

        _logger.LogWarning("Reservation page error: {Error}", result.Error);
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

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationPostDTO payload)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.CreateReservationAsync(payload);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Index), new { id = result.Value.ReservationId, result.Value.Token }, result.Value);

        _logger.LogWarning("Reservation creation error: {Error}", result.Error);
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

    [HttpPut]
    public async Task<IActionResult> UpdateReservation([FromBody] ReservationUpdateDTO payload)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _reservationService.UpdateReservationAsync(payload);

        if (result.IsSuccess)
            return Ok(result.Value);

        _logger.LogWarning("Reservation update error: {Error}", result.Error);

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

    [HttpDelete("{reservationId:guid}")]
    public async Task<IActionResult> CancelReservation([FromRoute] Guid reservationId, [FromQuery] string customerEmail, [FromQuery] string accessToken)
    {
        if (reservationId == Guid.Empty || string.IsNullOrWhiteSpace(customerEmail) || string.IsNullOrWhiteSpace(accessToken))
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "ReservationId, CustomerEmail, and AccessToken are required."
            });
        var result = await _reservationService.CancelReservationAsync(reservationId, customerEmail, accessToken);
        if (result.IsSuccess)
            return NoContent();
        _logger.LogWarning("Reservation cancellation error: {Error}", result.Error);
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

    [HttpGet("details")]
    public async Task<IActionResult> ReservationDetailsAsync([FromQuery] string accessToken, [FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(email))
            return BadRequest(new ProblemDetails
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "AccessToken and Email are required."
            });
        var result = await _reservationService.GetReservationDetails(accessToken, email);
        if (result.IsSuccess)
            return Ok(result.Value);
        _logger.LogWarning("Reservation details error: {Error}", result.Error);
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
