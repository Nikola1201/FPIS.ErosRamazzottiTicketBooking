using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class TicketServiceTests
{
    private static (TicketService svc, Mock<IUnitOfWork> uow, Mock<IRepository<ReservationTicket>> repo, Mock<IAppSettingsService> appSvc) Build()
    {
        var repo = new Mock<IRepository<ReservationTicket>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<ReservationTicket>()).Returns(repo.Object);
        var appSvc = new Mock<IAppSettingsService>();
        var logger = Mock.Of<ILogger<TicketService>>();
        return (new TicketService(uow.Object, logger, appSvc.Object), uow, repo, appSvc);
    }

    // ---------- ValidateZoneCapacitiesAsync ----------

    [Fact]
    public async Task ValidateZoneCapacitiesAsync_WhenZoneMissing_ReturnsInvalid()
    {
        var (svc, _, repo, _) = Build();
        var dateId = Guid.NewGuid();
        var missingZoneId = Guid.NewGuid();

        var (isValid, error) = await svc.ValidateZoneCapacitiesAsync(
            new[] { new TicketRequest { ZoneId = missingZoneId, Quantity = 1 } },
            dateId,
            new Dictionary<Guid, Zone>());

        Assert.False(isValid);
        Assert.Contains("not found", error);
    }

    [Fact]
    public async Task ValidateZoneCapacitiesAsync_WhenCapacityExceeded_ReturnsInvalid()
    {
        var (svc, _, repo, _) = Build();
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var zones = new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId, Name = "VIP", Capacity = 5 } };
        repo.Setup(r => r.CountAsync(It.IsAny<Expression<Func<ReservationTicket, bool>>?>()))
            .ReturnsAsync(4);

        var (isValid, error) = await svc.ValidateZoneCapacitiesAsync(
            new[] { new TicketRequest { ZoneId = zoneId, Quantity = 2 } },
            dateId,
            zones);

        Assert.False(isValid);
        Assert.Contains("Not enough capacity", error);
    }

    [Fact]
    public async Task ValidateZoneCapacitiesAsync_WithinCapacity_ReturnsValid()
    {
        var (svc, _, repo, _) = Build();
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var zones = new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId, Name = "VIP", Capacity = 100 } };
        repo.Setup(r => r.CountAsync(It.IsAny<Expression<Func<ReservationTicket, bool>>?>()))
            .ReturnsAsync(10);

        var (isValid, error) = await svc.ValidateZoneCapacitiesAsync(
            new[] { new TicketRequest { ZoneId = zoneId, Quantity = 5 } },
            dateId,
            zones);

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public async Task ValidateZoneCapacitiesAsync_NoTickets_ReturnsValid()
    {
        var (svc, _, _, _) = Build();
        var (isValid, error) = await svc.ValidateZoneCapacitiesAsync(
            Array.Empty<TicketRequest>(),
            Guid.NewGuid(),
            new Dictionary<Guid, Zone>());
        Assert.True(isValid);
        Assert.Null(error);
    }

    // ---------- GenerateTicketsAsync ----------

    [Fact]
    public async Task GenerateTicketsAsync_NoDiscounts_ProducesExpectedTickets()
    {
        var (svc, _, _, appSvc) = Build();
        appSvc.Setup(a => a.GetDiscountSettings()).ReturnsAsync((0, 0, 0, 0));
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var zones = new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId, Price = 100m } };
        var date = new ConcertDate { Id = dateId, Date = DateTime.UtcNow.AddYears(1) };

        var (tickets, discounts) = await svc.GenerateTicketsAsync(
            new[] { new TicketRequest { ZoneId = zoneId, Quantity = 3 } },
            dateId,
            zones,
            date,
            null);

        Assert.Equal(3, tickets.Count);
        Assert.All(tickets, t =>
        {
            Assert.Equal(zoneId, t.ZoneId);
            Assert.Equal(dateId, t.ConcertDateId);
            Assert.Equal(100m, t.Price);
        });
        Assert.Empty(discounts);
    }

    [Fact]
    public async Task GenerateTicketsAsync_EarlyBirdDiscount_AppliedToAllTickets()
    {
        var (svc, _, _, appSvc) = Build();
        appSvc.Setup(a => a.GetDiscountSettings()).ReturnsAsync((10, 30, 0, 0));
        var zoneId = Guid.NewGuid();
        var date = new ConcertDate { Date = DateTime.UtcNow.AddDays(60) };
        var zones = new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId, Price = 100m } };

        var (tickets, discounts) = await svc.GenerateTicketsAsync(
            new[] { new TicketRequest { ZoneId = zoneId, Quantity = 1 } },
            Guid.NewGuid(),
            zones,
            date,
            null);

        Assert.Single(tickets);
        Assert.Equal(90m, tickets[0].Price);
        Assert.Single(discounts);
        Assert.Equal(DiscountType.EarlyBird, discounts[0].Type);
    }

    [Fact]
    public async Task GenerateTicketsAsync_FifthTicketDiscount_AppliedOnEveryFifth()
    {
        var (svc, _, _, appSvc) = Build();
        appSvc.Setup(a => a.GetDiscountSettings()).ReturnsAsync((0, 0, 20, 0));
        var zoneId = Guid.NewGuid();
        var date = new ConcertDate { Date = DateTime.UtcNow };
        var zones = new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId, Price = 100m } };

        var (tickets, discounts) = await svc.GenerateTicketsAsync(
            new[] { new TicketRequest { ZoneId = zoneId, Quantity = 5 } },
            Guid.NewGuid(),
            zones,
            date,
            null);

        Assert.Equal(5, tickets.Count);
        // first 4 are full price, 5th has 20% off
        Assert.Equal(100m, tickets[0].Price);
        Assert.Equal(80m, tickets[4].Price);
        Assert.Single(discounts);
        Assert.Equal(DiscountType.FifthTicket, discounts[0].Type);
    }

    [Fact]
    public async Task GenerateTicketsAsync_FriendPromo_AppliedWhenPromoNotNull()
    {
        var (svc, _, _, appSvc) = Build();
        appSvc.Setup(a => a.GetDiscountSettings()).ReturnsAsync((0, 0, 0, 5));
        var zoneId = Guid.NewGuid();
        var zones = new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId, Price = 100m } };
        var promo = new PromoCode { Code = "ABCDEFGHIJ" };

        var (tickets, discounts) = await svc.GenerateTicketsAsync(
            new[] { new TicketRequest { ZoneId = zoneId, Quantity = 1 } },
            Guid.NewGuid(),
            zones,
            new ConcertDate { Date = DateTime.UtcNow },
            promo);

        Assert.Single(tickets);
        Assert.Equal(95m, tickets[0].Price);
        Assert.Single(discounts);
        Assert.Equal(DiscountType.FriendPromo, discounts[0].Type);
    }

    // ---------- GetTicketsByConcertDate ----------

    [Fact]
    public async Task GetTicketsByConcertDate_ReturnsListFromRepo()
    {
        var (svc, _, repo, _) = Build();
        var dateId = Guid.NewGuid();
        var tickets = new List<ReservationTicket> { new() { ConcertDateId = dateId } };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ReservationTicket, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<ReservationTicket, object>>[]>()))
            .ReturnsAsync(tickets);

        var result = await svc.GetTicketsByConcertDate(dateId);

        Assert.Single(result);
    }

    // ---------- GetTicketsByConcertDates ----------

    [Fact]
    public async Task GetTicketsByConcertDates_ReturnsListFromRepo()
    {
        var (svc, _, repo, _) = Build();
        var tickets = new List<ReservationTicket> { new(), new() };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ReservationTicket, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<ReservationTicket, object>>[]>()))
            .ReturnsAsync(tickets);

        var result = await svc.GetTicketsByConcertDates(new List<Guid> { Guid.NewGuid() });

        Assert.Equal(2, result.Count);
    }

    // ---------- Constructor ----------

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TicketService(null!, Mock.Of<ILogger<TicketService>>(), Mock.Of<IAppSettingsService>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TicketService(Mock.Of<IUnitOfWork>(), null!, Mock.Of<IAppSettingsService>()));
    }

    [Fact]
    public void Constructor_NullAppSettingsService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TicketService(Mock.Of<IUnitOfWork>(), Mock.Of<ILogger<TicketService>>(), null!));
    }
}
