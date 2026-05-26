using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class ReservationServiceTests
{
    private sealed class Mocks
    {
        public Mock<IUnitOfWork> Uow = new();
        public Mock<ICustomerService> CustomerSvc = new();
        public Mock<IPromoCodeService> PromoSvc = new();
        public Mock<ITicketService> TicketSvc = new();
        public Mock<ITokenService> TokenSvc = new();
        public Mock<IZoneService> ZoneSvc = new();
        public Mock<IRepository<Concert>> ConcertRepo = new();
        public Mock<IRepository<ConcertDate>> ConcertDateRepo = new();
        public Mock<IRepository<Zone>> ZoneRepo = new();
        public Mock<IRepository<AppSettings>> AppSettingsRepo = new();
        public Mock<IRepository<Reservation>> ReservationRepo = new();
        public Mock<IRepository<Customer>> CustomerRepo = new();
        public Mock<IRepository<PromoCode>> PromoCodeRepo = new();
        public Mock<IRepository<Discount>> DiscountRepo = new();
        public Mock<IRepository<ReservationTicket>> TicketRepo = new();
        public Mock<IDbContextTransaction> Transaction = new();
    }

    private static (ReservationService svc, Mocks m) Build()
    {
        var m = new Mocks();
        m.Uow.Setup(u => u.Repository<Concert>()).Returns(m.ConcertRepo.Object);
        m.Uow.Setup(u => u.Repository<ConcertDate>()).Returns(m.ConcertDateRepo.Object);
        m.Uow.Setup(u => u.Repository<Zone>()).Returns(m.ZoneRepo.Object);
        m.Uow.Setup(u => u.Repository<AppSettings>()).Returns(m.AppSettingsRepo.Object);
        m.Uow.Setup(u => u.Repository<Reservation>()).Returns(m.ReservationRepo.Object);
        m.Uow.Setup(u => u.Repository<Customer>()).Returns(m.CustomerRepo.Object);
        m.Uow.Setup(u => u.Repository<PromoCode>()).Returns(m.PromoCodeRepo.Object);
        m.Uow.Setup(u => u.Repository<Discount>()).Returns(m.DiscountRepo.Object);
        m.Uow.Setup(u => u.Repository<ReservationTicket>()).Returns(m.TicketRepo.Object);
        m.Uow.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(m.Transaction.Object);
        m.Uow.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        var logger = Mock.Of<ILogger<ReservationService>>();
        var svc = new ReservationService(
            m.Uow.Object,
            logger,
            m.CustomerSvc.Object,
            m.PromoSvc.Object,
            m.TicketSvc.Object,
            m.TokenSvc.Object,
            m.ZoneSvc.Object);
        return (svc, m);
    }

    // ---------- Constructor null-arg checks ----------

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            null!, Mock.Of<ILogger<ReservationService>>(),
            Mock.Of<ICustomerService>(), Mock.Of<IPromoCodeService>(),
            Mock.Of<ITicketService>(), Mock.Of<ITokenService>(), Mock.Of<IZoneService>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            Mock.Of<IUnitOfWork>(), null!,
            Mock.Of<ICustomerService>(), Mock.Of<IPromoCodeService>(),
            Mock.Of<ITicketService>(), Mock.Of<ITokenService>(), Mock.Of<IZoneService>()));
    }

    [Fact]
    public void Constructor_NullCustomerService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            Mock.Of<IUnitOfWork>(), Mock.Of<ILogger<ReservationService>>(),
            null!, Mock.Of<IPromoCodeService>(),
            Mock.Of<ITicketService>(), Mock.Of<ITokenService>(), Mock.Of<IZoneService>()));
    }

    [Fact]
    public void Constructor_NullPromoCodeService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            Mock.Of<IUnitOfWork>(), Mock.Of<ILogger<ReservationService>>(),
            Mock.Of<ICustomerService>(), null!,
            Mock.Of<ITicketService>(), Mock.Of<ITokenService>(), Mock.Of<IZoneService>()));
    }

    [Fact]
    public void Constructor_NullTicketService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            Mock.Of<IUnitOfWork>(), Mock.Of<ILogger<ReservationService>>(),
            Mock.Of<ICustomerService>(), Mock.Of<IPromoCodeService>(),
            null!, Mock.Of<ITokenService>(), Mock.Of<IZoneService>()));
    }

    [Fact]
    public void Constructor_NullTokenService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            Mock.Of<IUnitOfWork>(), Mock.Of<ILogger<ReservationService>>(),
            Mock.Of<ICustomerService>(), Mock.Of<IPromoCodeService>(),
            Mock.Of<ITicketService>(), null!, Mock.Of<IZoneService>()));
    }

    [Fact]
    public void Constructor_NullZoneService_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new ReservationService(
            Mock.Of<IUnitOfWork>(), Mock.Of<ILogger<ReservationService>>(),
            Mock.Of<ICustomerService>(), Mock.Of<IPromoCodeService>(),
            Mock.Of<ITicketService>(), Mock.Of<ITokenService>(), null!));
    }

    // ---------- GetReservationPage ----------

    [Fact]
    public async Task GetReservationPage_WhenConcertExists_ReturnsSuccess()
    {
        var (svc, m) = Build();
        var concert = new Concert { Name = "Tour", Dates = new List<ConcertDate>() };
        m.ConcertRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Concert, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Concert, object>>[]>()))
            .ReturnsAsync(new List<Concert> { concert });
        m.ZoneRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Zone, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Zone, object>>[]>()))
            .ReturnsAsync(new List<Zone>());
        m.TicketSvc.Setup(s => s.GetTicketsByConcertDates(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<ReservationTicket>());
        m.AppSettingsRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<AppSettings, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<AppSettings, object>>[]>()))
            .ReturnsAsync(new List<AppSettings>());

        var result = await svc.GetReservationPage();

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task GetReservationPage_WhenNoConcert_Returns404()
    {
        var (svc, m) = Build();
        m.ConcertRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Concert, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Concert, object>>[]>()))
            .ReturnsAsync(new List<Concert>());

        var result = await svc.GetReservationPage();

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.ErrorCode);
    }

    [Fact]
    public async Task GetReservationPage_WhenRepoThrows_Returns500()
    {
        var (svc, m) = Build();
        m.ConcertRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Concert, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Concert, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.GetReservationPage();

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.ErrorCode);
    }

    // ---------- CreateReservationAsync ----------

    [Fact]
    public async Task CreateReservationAsync_WhenConcertDateMissing_Returns404()
    {
        var (svc, m) = Build();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ConcertDate?)null);

        var result = await svc.CreateReservationAsync(new ReservationPostDTO { ConcertDateId = Guid.NewGuid() });

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.ErrorCode);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenNoZones_Returns400()
    {
        var (svc, m) = Build();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ConcertDate());
        m.ZoneSvc.Setup(z => z.GetAllZones()).ReturnsAsync(new Dictionary<Guid, Zone>());

        var result = await svc.CreateReservationAsync(new ReservationPostDTO { ConcertDateId = Guid.NewGuid() });

        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.ErrorCode);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenValidationFails_Returns400()
    {
        var (svc, m) = Build();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ConcertDate());
        m.ZoneSvc.Setup(z => z.GetAllZones()).ReturnsAsync(new Dictionary<Guid, Zone> { [Guid.NewGuid()] = new Zone() });
        m.TicketSvc.Setup(t => t.ValidateZoneCapacitiesAsync(
                It.IsAny<IEnumerable<TicketRequest>>(),
                It.IsAny<Guid>(),
                It.IsAny<IDictionary<Guid, Zone>>(),
                It.IsAny<Guid?>()))
            .ReturnsAsync((false, "capacity exceeded"));

        var result = await svc.CreateReservationAsync(new ReservationPostDTO
        {
            ConcertDateId = Guid.NewGuid(),
            Tickets = [new TicketRequest()]
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.ErrorCode);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenInvalidPromoCode_Returns400()
    {
        var (svc, m) = Build();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ConcertDate());
        m.ZoneSvc.Setup(z => z.GetAllZones()).ReturnsAsync(new Dictionary<Guid, Zone> { [Guid.NewGuid()] = new Zone() });
        m.TicketSvc.Setup(t => t.ValidateZoneCapacitiesAsync(
                It.IsAny<IEnumerable<TicketRequest>>(),
                It.IsAny<Guid>(),
                It.IsAny<IDictionary<Guid, Zone>>(),
                It.IsAny<Guid?>()))
            .ReturnsAsync((true, (string?)null));
        m.PromoSvc.Setup(p => p.IsValidPromoCodeAsync(It.IsAny<string>())).ReturnsAsync((PromoCode?)null);

        var result = await svc.CreateReservationAsync(new ReservationPostDTO
        {
            ConcertDateId = Guid.NewGuid(),
            PromoCode = "BADPROMO00",
            Tickets = [new TicketRequest()]
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.ErrorCode);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenCustomerExists_Returns400()
    {
        var (svc, m) = Build();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ConcertDate());
        m.ZoneSvc.Setup(z => z.GetAllZones()).ReturnsAsync(new Dictionary<Guid, Zone> { [Guid.NewGuid()] = new Zone() });
        m.TicketSvc.Setup(t => t.ValidateZoneCapacitiesAsync(
                It.IsAny<IEnumerable<TicketRequest>>(),
                It.IsAny<Guid>(),
                It.IsAny<IDictionary<Guid, Zone>>(),
                It.IsAny<Guid?>()))
            .ReturnsAsync((true, (string?)null));
        m.CustomerSvc.Setup(c => c.CreateCustomer(It.IsAny<CustomerCreateDTO>())).Returns((Customer?)null);

        var result = await svc.CreateReservationAsync(new ReservationPostDTO
        {
            ConcertDateId = Guid.NewGuid(),
            Tickets = [new TicketRequest()]
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.ErrorCode);
    }

    [Fact]
    public async Task CreateReservationAsync_HappyPath_ReturnsSuccessWithReservationIdAndToken()
    {
        var (svc, m) = Build();
        var zoneId = Guid.NewGuid();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ConcertDate());
        m.ZoneSvc.Setup(z => z.GetAllZones()).ReturnsAsync(new Dictionary<Guid, Zone> { [zoneId] = new Zone { Id = zoneId } });
        m.TicketSvc.Setup(t => t.ValidateZoneCapacitiesAsync(
                It.IsAny<IEnumerable<TicketRequest>>(),
                It.IsAny<Guid>(),
                It.IsAny<IDictionary<Guid, Zone>>(),
                It.IsAny<Guid?>()))
            .ReturnsAsync((true, (string?)null));
        m.CustomerSvc.Setup(c => c.CreateCustomer(It.IsAny<CustomerCreateDTO>()))
            .Returns(new Customer { Id = Guid.NewGuid() });
        m.TicketSvc.Setup(t => t.GenerateTicketsAsync(
                It.IsAny<IEnumerable<TicketRequest>>(),
                It.IsAny<Guid>(),
                It.IsAny<IDictionary<Guid, Zone>>(),
                It.IsAny<ConcertDate>(),
                It.IsAny<PromoCode?>()))
            .ReturnsAsync((new List<ReservationTicket>(), new List<Discount>()));
        m.TokenSvc.Setup(t => t.CreateToken(It.IsAny<Guid>()))
            .ReturnsAsync(new AccessToken { Value = "mytoken", Id = Guid.NewGuid() });
        m.PromoSvc.Setup(p => p.GeneratePromoCode(It.IsAny<Guid>()))
            .ReturnsAsync(new PromoCode { Id = Guid.NewGuid(), Code = "GEN0000000" });

        var result = await svc.CreateReservationAsync(new ReservationPostDTO
        {
            ConcertDateId = Guid.NewGuid(),
            Tickets = [new TicketRequest { ZoneId = zoneId, Quantity = 1 }]
        });

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("mytoken", result.Value!.Token);
        Assert.NotEqual(Guid.Empty, result.Value.ReservationId);
    }

    [Fact]
    public async Task CreateReservationAsync_WhenExceptionThrown_Returns500()
    {
        var (svc, m) = Build();
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.CreateReservationAsync(new ReservationPostDTO { ConcertDateId = Guid.NewGuid() });

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.ErrorCode);
    }

    // ---------- UpdateReservationAsync ----------

    [Fact]
    public async Task UpdateReservationAsync_WhenReservationCancelled_Returns400()
    {
        var (svc, m) = Build();
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = ReservationStatus.Cancelled,
            Tickets = new List<ReservationTicket> { new() { ConcertDateId = Guid.NewGuid() } }
        };
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ReturnsAsync(new List<Reservation> { reservation });

        var result = await svc.UpdateReservationAsync(new ReservationUpdateDTO
        {
            CustomerEmail = "a@b.rs",
            AccessToken = "tok",
            Tickets = [new TicketRequest()]
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(400, result.ErrorCode);
    }

    [Fact]
    public async Task UpdateReservationAsync_WhenConcertDateMissing_Returns404()
    {
        var (svc, m) = Build();
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = ReservationStatus.Active,
            Tickets = new List<ReservationTicket> { new() { ConcertDateId = Guid.NewGuid() } }
        };
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ReturnsAsync(new List<Reservation> { reservation });
        m.ConcertDateRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ConcertDate?)null);

        var result = await svc.UpdateReservationAsync(new ReservationUpdateDTO
        {
            CustomerEmail = "a@b.rs",
            AccessToken = "tok",
            Tickets = [new TicketRequest()]
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.ErrorCode);
    }

    [Fact]
    public async Task UpdateReservationAsync_WhenExceptionThrown_Returns500()
    {
        var (svc, m) = Build();
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.UpdateReservationAsync(new ReservationUpdateDTO
        {
            CustomerEmail = "a@b.rs",
            AccessToken = "tok",
            Tickets = [new TicketRequest()]
        });

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.ErrorCode);
    }

    // ---------- CancelReservationAsync ----------

    [Fact]
    public async Task CancelReservationAsync_WhenReservationMissing_Returns404()
    {
        var (svc, m) = Build();
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ReturnsAsync(new List<Reservation>());

        var result = await svc.CancelReservationAsync(Guid.NewGuid(), "a@b.rs", "tok");

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.ErrorCode);
    }

    [Fact]
    public async Task CancelReservationAsync_HappyPath_ReturnsSuccess()
    {
        var (svc, m) = Build();
        var id = Guid.NewGuid();
        var reservation = new Reservation
        {
            Id = id,
            Customer = new Customer { Email = "a@b.rs" },
            AccessToken = new AccessToken { Value = "tok" },
            Tickets = new List<ReservationTicket>(),
            Discounts = new List<Discount>(),
            GeneratedPromoCode = new PromoCode { Id = Guid.NewGuid(), IsUsed = false },
            UsedPromoCode = null
        };
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ReturnsAsync(new List<Reservation> { reservation });

        var result = await svc.CancelReservationAsync(id, "a@b.rs", "tok");

        Assert.True(result.IsSuccess);
        Assert.Equal(id, result.Value!.ReservationId);
        Assert.True(result.Value.Cancelled);
        Assert.Equal(ReservationStatus.Cancelled, reservation.Status);
    }

    [Fact]
    public async Task CancelReservationAsync_WhenExceptionThrown_Returns500()
    {
        var (svc, m) = Build();
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.CancelReservationAsync(Guid.NewGuid(), "a@b.rs", "tok");

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.ErrorCode);
    }

    // ---------- GetReservationDetails ----------

    [Fact]
    public async Task GetReservationDetails_WhenMissing_Returns404()
    {
        var (svc, m) = Build();
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ReturnsAsync(new List<Reservation>());

        var result = await svc.GetReservationDetails("tok", "a@b.rs");

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.ErrorCode);
    }

    [Fact]
    public async Task GetReservationDetails_WhenExceptionThrown_Returns500()
    {
        var (svc, m) = Build();
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = await svc.GetReservationDetails("tok", "a@b.rs");

        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.ErrorCode);
    }

    [Fact]
    public async Task GetReservationDetails_HappyPath_ReturnsSuccessWithViewModel()
    {
        var (svc, m) = Build();
        // The mapping ReservationDetailsMappings.ToReservationDetailsViewModel dereferences
        // concertDate.Tickets and reservation.GeneratedPromoCode without null-checks. To exercise
        // the happy path we provide non-null instances for both.
        var dateId = Guid.NewGuid();
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Status = ReservationStatus.Active,
            Customer = new Customer { FirstName = "A", LastName = "B", Email = "a@b.rs" },
            AccessToken = new AccessToken { Value = "tok" },
            Tickets = new List<ReservationTicket> { new() { ConcertDateId = dateId } },
            Discounts = new List<Discount>(),
            GeneratedPromoCode = new PromoCode { Id = Guid.NewGuid(), Code = "GEN0000000", IsUsed = false }
        };
        m.ReservationRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Reservation, object>>[]>()))
            .ReturnsAsync(new List<Reservation> { reservation });
        m.ConcertDateRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ConcertDate, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<ConcertDate, object>>[]>()))
            .ReturnsAsync(new List<ConcertDate> { new() { Id = dateId, Tickets = new List<ReservationTicket>() } });
        m.ZoneRepo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Zone, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Zone, object>>[]>()))
            .ReturnsAsync(new List<Zone>());
        m.TicketSvc.Setup(t => t.GetTicketsByConcertDates(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<ReservationTicket>());

        var result = await svc.GetReservationDetails("tok", "a@b.rs");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(reservation.Id, result.Value!.ReservationId);
    }
}
