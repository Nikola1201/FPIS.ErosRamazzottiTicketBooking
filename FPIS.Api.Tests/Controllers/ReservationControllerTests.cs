using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Controllers;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Controllers;

public class ReservationControllerTests
{
    private static (ReservationController controller, Mock<IReservationService> svc) Build()
    {
        var svc = new Mock<IReservationService>();
        var logger = Mock.Of<ILogger<ReservationController>>();
        return (new ReservationController(svc.Object, logger), svc);
    }

    [Fact]
    public void Constructor_NullService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationController(null!, Mock.Of<ILogger<ReservationController>>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationController(Mock.Of<IReservationService>(), null!));
    }

    // ---------- GET /api/reservation ----------

    [Fact]
    public async Task Index_Success_ReturnsOkWithViewModel()
    {
        var (controller, svc) = Build();
        var vm = new ReservationPageViewModel { Title = "Page" };
        svc.Setup(s => s.GetReservationPage()).ReturnsAsync(Result<ReservationPageViewModel>.Success(vm));

        var result = await controller.Index();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(vm, ok.Value);
    }

    [Fact]
    public async Task Index_404_ReturnsNotFound()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetReservationPage()).ReturnsAsync(Result<ReservationPageViewModel>.Failure("missing", 404));

        var result = await controller.Index();

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Index_500_ReturnsServerError()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetReservationPage()).ReturnsAsync(Result<ReservationPageViewModel>.Failure("boom", 500));

        var result = await controller.Index();

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }

    // ---------- POST /api/reservation ----------

    [Fact]
    public async Task CreateReservation_InvalidModelState_ReturnsBadRequest()
    {
        var (controller, _) = Build();
        controller.ModelState.AddModelError("Customer.FirstName", "required");

        var result = await controller.CreateReservation(new ReservationPostDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateReservation_Success_ReturnsCreatedAtAction()
    {
        var (controller, svc) = Build();
        var dto = new ReservationPostDTO();
        var resultDto = new ReservationResultDTO { ReservationId = Guid.NewGuid(), Token = "tok" };
        svc.Setup(s => s.CreateReservationAsync(dto)).ReturnsAsync(Result<ReservationResultDTO>.Success(resultDto));

        var result = await controller.CreateReservation(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Same(resultDto, created.Value);
    }

    [Fact]
    public async Task CreateReservation_404_ReturnsNotFound()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.CreateReservationAsync(It.IsAny<ReservationPostDTO>()))
           .ReturnsAsync(Result<ReservationResultDTO>.Failure("missing", 404));

        var result = await controller.CreateReservation(new ReservationPostDTO());

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateReservation_500_ReturnsServerError()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.CreateReservationAsync(It.IsAny<ReservationPostDTO>()))
           .ReturnsAsync(Result<ReservationResultDTO>.Failure("boom", 500));

        var result = await controller.CreateReservation(new ReservationPostDTO());

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }

    // ---------- PUT /api/reservation ----------

    [Fact]
    public async Task UpdateReservation_InvalidModelState_ReturnsBadRequest()
    {
        var (controller, _) = Build();
        controller.ModelState.AddModelError("CustomerEmail", "required");

        var result = await controller.UpdateReservation(new ReservationUpdateDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateReservation_Success_ReturnsOk()
    {
        var (controller, svc) = Build();
        var dto = new ReservationUpdateDTO { CustomerEmail = "a@b.rs", AccessToken = "tok", Tickets = [new()] };
        var resultDto = new ReservationUpdateResultDTO { Updated = true, Status = "Modified", Message = "ok" };
        svc.Setup(s => s.UpdateReservationAsync(dto)).ReturnsAsync(Result<ReservationUpdateResultDTO>.Success(resultDto));

        var result = await controller.UpdateReservation(dto);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(resultDto, ok.Value);
    }

    [Fact]
    public async Task UpdateReservation_404_ReturnsNotFound()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.UpdateReservationAsync(It.IsAny<ReservationUpdateDTO>()))
           .ReturnsAsync(Result<ReservationUpdateResultDTO>.Failure("missing", 404));

        var result = await controller.UpdateReservation(new ReservationUpdateDTO { CustomerEmail = "x", AccessToken = "y", Tickets = [new()] });

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateReservation_500_ReturnsServerError()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.UpdateReservationAsync(It.IsAny<ReservationUpdateDTO>()))
           .ReturnsAsync(Result<ReservationUpdateResultDTO>.Failure("boom", 500));

        var result = await controller.UpdateReservation(new ReservationUpdateDTO { CustomerEmail = "x", AccessToken = "y", Tickets = [new()] });

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }

    // ---------- DELETE /api/reservation/{id} ----------

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000", "a@b.rs", "tok")]
    [InlineData("11111111-1111-1111-1111-111111111111", "", "tok")]
    [InlineData("11111111-1111-1111-1111-111111111111", "a@b.rs", "")]
    public async Task CancelReservation_MissingArgs_ReturnsBadRequest(string idStr, string email, string token)
    {
        var (controller, _) = Build();
        var id = Guid.Parse(idStr);

        var result = await controller.CancelReservation(id, email, token);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CancelReservation_Success_ReturnsNoContent()
    {
        var (controller, svc) = Build();
        var id = Guid.NewGuid();
        svc.Setup(s => s.CancelReservationAsync(id, "a@b.rs", "tok"))
           .ReturnsAsync(Result<ReservationCancelResultDTO>.Success(new ReservationCancelResultDTO { ReservationId = id, Cancelled = true }));

        var result = await controller.CancelReservation(id, "a@b.rs", "tok");

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task CancelReservation_404_ReturnsNotFound()
    {
        var (controller, svc) = Build();
        var id = Guid.NewGuid();
        svc.Setup(s => s.CancelReservationAsync(id, "a@b.rs", "tok"))
           .ReturnsAsync(Result<ReservationCancelResultDTO>.Failure("missing", 404));

        var result = await controller.CancelReservation(id, "a@b.rs", "tok");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CancelReservation_500_ReturnsServerError()
    {
        var (controller, svc) = Build();
        var id = Guid.NewGuid();
        svc.Setup(s => s.CancelReservationAsync(id, "a@b.rs", "tok"))
           .ReturnsAsync(Result<ReservationCancelResultDTO>.Failure("boom", 500));

        var result = await controller.CancelReservation(id, "a@b.rs", "tok");

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }

    // ---------- GET /api/reservation/details ----------

    [Theory]
    [InlineData("", "a@b.rs")]
    [InlineData("tok", "")]
    public async Task ReservationDetails_MissingArgs_ReturnsBadRequest(string token, string email)
    {
        var (controller, _) = Build();
        var result = await controller.ReservationDetailsAsync(token, email);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ReservationDetails_Success_ReturnsOkWithDetails()
    {
        var (controller, svc) = Build();
        var details = new ReservationDetailsViewModel { ReservationId = Guid.NewGuid(), Status = "Active" };
        svc.Setup(s => s.GetReservationDetails("tok", "a@b.rs"))
           .ReturnsAsync(Result<ReservationDetailsViewModel>.Success(details));

        var result = await controller.ReservationDetailsAsync("tok", "a@b.rs");

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(details, ok.Value);
    }

    [Fact]
    public async Task ReservationDetails_404_ReturnsNotFound()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetReservationDetails(It.IsAny<string>(), It.IsAny<string>()))
           .ReturnsAsync(Result<ReservationDetailsViewModel>.Failure("missing", 404));

        var result = await controller.ReservationDetailsAsync("tok", "a@b.rs");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ReservationDetails_500_ReturnsServerError()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetReservationDetails(It.IsAny<string>(), It.IsAny<string>()))
           .ReturnsAsync(Result<ReservationDetailsViewModel>.Failure("boom", 500));

        var result = await controller.ReservationDetailsAsync("tok", "a@b.rs");

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }
}
