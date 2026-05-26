using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class ZoneServiceTests
{
    private static (ZoneService svc, Mock<IUnitOfWork> uow, Mock<IRepository<Zone>> repo) Build()
    {
        var repo = new Mock<IRepository<Zone>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<Zone>()).Returns(repo.Object);
        var logger = Mock.Of<ILogger<ZoneService>>();
        return (new ZoneService(uow.Object, logger), uow, repo);
    }

    [Fact]
    public async Task GetAllZones_ReturnsDictionaryKeyedById()
    {
        var (svc, _, repo) = Build();
        var z1 = new Zone { Id = Guid.NewGuid(), Name = "VIP" };
        var z2 = new Zone { Id = Guid.NewGuid(), Name = "Standing" };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Zone, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Zone, object>>[]>()))
            .ReturnsAsync(new List<Zone> { z1, z2 });

        var result = await svc.GetAllZones();

        Assert.Equal(2, result.Count);
        Assert.Same(z1, result[z1.Id]);
        Assert.Same(z2, result[z2.Id]);
    }

    [Fact]
    public async Task GetAllZones_WhenRepoThrows_ReturnsEmptyDictionary()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Zone, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Zone, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.GetAllZones();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllZones_NoZonesInDb_ReturnsEmptyDictionary()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Zone, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Zone, object>>[]>()))
            .ReturnsAsync(new List<Zone>());

        var result = await svc.GetAllZones();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ZoneService(null!, Mock.Of<ILogger<ZoneService>>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ZoneService(Mock.Of<IUnitOfWork>(), null!));
    }
}
