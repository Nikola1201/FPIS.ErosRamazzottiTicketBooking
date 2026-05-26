using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class PromoCodeServiceTests
{
    private static (PromoCodeService svc, Mock<IUnitOfWork> uow, Mock<IRepository<PromoCode>> repo) Build()
    {
        var repo = new Mock<IRepository<PromoCode>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<PromoCode>()).Returns(repo.Object);
        var logger = Mock.Of<ILogger<PromoCodeService>>();
        return (new PromoCodeService(logger, uow.Object), uow, repo);
    }

    // ---------- ApplyPromoCode ----------

    [Fact]
    public void ApplyPromoCode_NullPromo_DoesNothing()
    {
        var (svc, _, repo) = Build();
        svc.ApplyPromoCode(Guid.NewGuid(), null);
        repo.Verify(r => r.Update(It.IsAny<PromoCode>()), Times.Never);
    }

    [Fact]
    public void ApplyPromoCode_ValidPromo_MarksUsedAndUpdates()
    {
        var (svc, _, repo) = Build();
        var promo = new PromoCode { Id = Guid.NewGuid(), Code = "ABCDEFGHIJ", IsUsed = false };
        var reservationId = Guid.NewGuid();

        svc.ApplyPromoCode(reservationId, promo);

        Assert.True(promo.IsUsed);
        Assert.Equal(reservationId, promo.UsedByReservationId);
        repo.Verify(r => r.Update(promo), Times.Once);
    }

    [Fact]
    public void ApplyPromoCode_WhenUpdateThrows_DoesNotPropagate()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.Update(It.IsAny<PromoCode>())).Throws(new InvalidOperationException("boom"));
        var promo = new PromoCode { Id = Guid.NewGuid(), Code = "X" };

        // Should not throw
        svc.ApplyPromoCode(Guid.NewGuid(), promo);
    }

    // ---------- GeneratePromoCode ----------

    [Fact]
    public async Task GeneratePromoCode_Success_ReturnsCodeWithExpectedFields()
    {
        var (svc, _, repo) = Build();
        var reservationId = Guid.NewGuid();

        var promo = await svc.GeneratePromoCode(reservationId);

        Assert.NotNull(promo);
        Assert.NotEqual(Guid.Empty, promo.Id);
        Assert.False(promo.IsUsed);
        Assert.Equal(reservationId, promo.GeneratedByReservationId);
        Assert.Equal(10, promo.Code!.Length);
        repo.Verify(r => r.AddAsync(It.IsAny<PromoCode>()), Times.Once);
    }

    [Fact]
    public async Task GeneratePromoCode_WhenAddThrows_ReturnsEmptyPromoCode()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.AddAsync(It.IsAny<PromoCode>())).ThrowsAsync(new InvalidOperationException("boom"));

        var promo = await svc.GeneratePromoCode(Guid.NewGuid());

        // service catches and returns new()
        Assert.NotNull(promo);
        Assert.Equal(Guid.Empty, promo.Id);
    }

    // ---------- IsValidPromoCodeAsync ----------

    [Fact]
    public async Task IsValidPromoCodeAsync_WhenCodeExistsAndUnused_ReturnsPromo()
    {
        var (svc, _, repo) = Build();
        var existing = new PromoCode { Code = "VALID12345", IsUsed = false };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<PromoCode, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<PromoCode, object>>[]>()))
            .ReturnsAsync(new List<PromoCode> { existing });

        var result = await svc.IsValidPromoCodeAsync("VALID12345");

        Assert.Same(existing, result);
    }

    [Fact]
    public async Task IsValidPromoCodeAsync_WhenCodeNotFound_ReturnsNull()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<PromoCode, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<PromoCode, object>>[]>()))
            .ReturnsAsync(new List<PromoCode>());

        var result = await svc.IsValidPromoCodeAsync("DOESNT0000");

        Assert.Null(result);
    }

    [Fact]
    public async Task IsValidPromoCodeAsync_WhenRepoThrows_ReturnsNull()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<PromoCode, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<PromoCode, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.IsValidPromoCodeAsync("X");

        Assert.Null(result);
    }

    // ---------- Constructor ----------

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new PromoCodeService(null!, Mock.Of<IUnitOfWork>()));
    }

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new PromoCodeService(Mock.Of<ILogger<PromoCodeService>>(), null!));
    }
}
