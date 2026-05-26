using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Controllers;

public class HomeControllerTests
{
    private static (HomeController controller, Mock<IHomeService> svc) Build()
    {
        var svc = new Mock<IHomeService>();
        var logger = Mock.Of<ILogger<HomeController>>();
        return (new HomeController(svc.Object, logger), svc);
    }

    [Fact]
    public async Task Index_Success_ReturnsOkWithViewModel()
    {
        var (controller, svc) = Build();
        var vm = new HomePageViewModel { Title = "Welcome" };
        svc.Setup(s => s.GetHomePage()).ReturnsAsync(Result<HomePageViewModel>.Success(vm));

        var result = await controller.Index();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(vm, ok.Value);
        svc.Verify(s => s.GetHomePage(), Times.Once);
    }

    [Fact]
    public async Task Index_404Error_ReturnsNotFound()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetHomePage()).ReturnsAsync(Result<HomePageViewModel>.Failure("No concert", 404));

        var result = await controller.Index();

        var nf = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(nf.Value);
    }

    [Fact]
    public async Task Index_500Error_ReturnsInternalServerError()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetHomePage()).ReturnsAsync(Result<HomePageViewModel>.Failure("Boom", 500));

        var result = await controller.Index();

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }

    [Fact]
    public async Task Index_NullErrorCode_DefaultsTo500()
    {
        var (controller, svc) = Build();
        svc.Setup(s => s.GetHomePage()).ReturnsAsync(Result<HomePageViewModel>.Failure("Generic error"));

        var result = await controller.Index();

        var status = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, status.StatusCode);
    }

    [Fact]
    public void Constructor_NullHomeService_Throws()
    {
        var logger = Mock.Of<ILogger<HomeController>>();
        Assert.Throws<ArgumentNullException>(() => new HomeController(null!, logger));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        var svc = Mock.Of<IHomeService>();
        Assert.Throws<ArgumentNullException>(() => new HomeController(svc, null!));
    }
}
