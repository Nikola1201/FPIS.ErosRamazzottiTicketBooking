using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Controllers;

/// <summary>
/// API kontroler za rezervacije: prikaz stranice rezervacije, kreiranje, izmena, otkazivanje i detalji rezervacije.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly ILogger<ReservationController> _logger;

    /// <summary>Konstruktor sa injektovanim servisom i logger-om.</summary>
    /// <param name="reservationService">Servis za rad sa rezervacijama.</param>
    /// <param name="logger">Logger za greške i upozorenja.</param>
    public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>GET /api/reservation — vraća <see cref="ReservationPageViewModel"/> sa podacima o koncertu i raspoloživim zonama.</summary>
    /// <returns>200 OK sa view modelom ili 404/500 sa <see cref="ProblemDetails"/>.</returns>
    /// <response code="200">Uspešno vraćena stranica rezervacije.</response>
    /// <response code="404">Nije pronađen koncert.</response>
    /// <response code="500">Interna greška servera.</response>
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

    /// <summary>POST /api/reservation — kreira novu rezervaciju na osnovu <see cref="ReservationPostDTO"/>.</summary>
    /// <param name="payload">DTO sa podacima o kupcu, datumu koncerta, kartama i opcionim promo kodom.</param>
    /// <returns>201 Created sa <see cref="ReservationResultDTO"/> ili odgovarajući error response.</returns>
    /// <response code="201">Rezervacija uspešno kreirana.</response>
    /// <response code="400">Validacija nije uspela ili je zahtev nevažeći.</response>
    /// <response code="404">Datum koncerta nije pronađen.</response>
    /// <response code="500">Interna greška servera.</response>
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

    /// <summary>PUT /api/reservation — menja postojeću rezervaciju.</summary>
    /// <param name="payload">DTO sa email-om, pristupnim tokenom i novim kartama.</param>
    /// <returns>200 OK sa <see cref="ReservationUpdateResultDTO"/> ili odgovarajući error response.</returns>
    /// <response code="200">Rezervacija uspešno izmenjena.</response>
    /// <response code="400">Validacija nije uspela ili je zahtev nevažeći.</response>
    /// <response code="404">Rezervacija nije pronađena ili je access token neispravan.</response>
    /// <response code="500">Interna greška servera.</response>
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

    /// <summary>DELETE /api/reservation/{reservationId} — otkazuje rezervaciju.</summary>
    /// <param name="reservationId">Identifikator rezervacije.</param>
    /// <param name="customerEmail">Email kupca (autorizacija).</param>
    /// <param name="accessToken">Pristupni token rezervacije (autorizacija).</param>
    /// <returns>204 No Content u slučaju uspeha; inače odgovarajući error response.</returns>
    /// <response code="204">Rezervacija uspešno otkazana.</response>
    /// <response code="400">Nedostaju obavezni parametri.</response>
    /// <response code="404">Rezervacija nije pronađena ili je access token neispravan.</response>
    /// <response code="500">Interna greška servera.</response>
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

    /// <summary>GET /api/reservation/details — vraća detalje rezervacije.</summary>
    /// <param name="accessToken">Pristupni token rezervacije (autorizacija).</param>
    /// <param name="email">Email kupca (autorizacija).</param>
    /// <returns>200 OK sa <see cref="ReservationDetailsViewModel"/> ili odgovarajući error response.</returns>
    /// <response code="200">Detalji rezervacije uspešno vraćeni.</response>
    /// <response code="400">Nedostaju obavezni parametri.</response>
    /// <response code="404">Rezervacija nije pronađena ili je access token neispravan.</response>
    /// <response code="500">Interna greška servera.</response>
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
