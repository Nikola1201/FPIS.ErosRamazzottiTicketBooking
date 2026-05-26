using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class HomeServiceTests
{
    private static (HomeService svc, Mock<IUnitOfWork> uow, Mock<IRepository<Concert>> repo) Build()
    {
        var repo = new Mock<IRepository<Concert>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<Concert>()).Returns(repo.Object);
        var logger = Mock.Of<ILogger<HomeService>>();
        return (new HomeService(uow.Object, logger), uow, repo);
    }

    [Fact]
    public async Task GetHomePage_WhenConcertExists_ReturnsSuccessWithViewModel()
    {
        var (svc, _, repo) = Build();
        var concert = new Concert
        {
            Name = "Tour",
            City = "Beograd",
            Venue = "Arena",
            Address = "X",
            Dates = new List<ConcertDate>()
        };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Concert, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Concert, object>>[]>()))
            .ReturnsAsync(new List<Concert> { concert });

        var result = await svc.GetHomePage();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Tour", result.Value!.Concert!.Title);
        Assert.Equal("Welcome to Eros Ramazzotti Live!", result.Value.Title);
    }

    [Fact]
    public async Task GetHomePage_WhenNoConcert_Returns404Failure()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Concert, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Concert, object>>[]>()))
            .ReturnsAsync(new List<Concert>());

        var result = await svc.GetHomePage();

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.ErrorCode);
    }

    [Fact]
    public async Task GetHomePage_WhenRepoThrows_Returns500Failure()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Concert, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Concert, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("DB down"));

        var result = await svc.GetHomePage();

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.ErrorCode);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeService(null!, Mock.Of<ILogger<HomeService>>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new HomeService(Mock.Of<IUnitOfWork>(), null!));
    }
}
