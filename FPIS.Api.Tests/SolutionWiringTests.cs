using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests;

public class SolutionWiringTests
{
    [Fact]
    public void HomeController_CanBeConstructed_WithMockedDependencies()
    {
        var homeService = new Mock<IHomeService>();
        var logger = new Mock<ILogger<HomeController>>();

        var controller = new HomeController(homeService.Object, logger.Object);

        Assert.NotNull(controller);
    }
}
