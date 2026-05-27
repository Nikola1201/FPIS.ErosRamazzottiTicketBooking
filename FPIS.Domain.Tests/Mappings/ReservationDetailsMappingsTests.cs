using FPIS.Domain.Mappings;
using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Mappings;

public class ReservationDetailsMappingsTests
{
    private static Reservation BuildReservation(Guid zoneId, Guid dateId)
    {
        return new Reservation
        {
            Id = Guid.NewGuid(),
            Status = ReservationStatus.Active,
            Customer = new Customer { FirstName = "Jovan", LastName = "Jovanović", Email = "j@x.rs" },
            AccessToken = new AccessToken { Value = "tok-abc" },
            GeneratedPromoCode = new PromoCode { Code = "GEN1234567", IsUsed = false },
            UsedPromoCode = new PromoCode { Code = "USE1234567" },
            Tickets =
            [
                new ReservationTicket { Id = Guid.NewGuid(), ZoneId = zoneId, ConcertDateId = dateId, Price = 100m },
                new ReservationTicket { Id = Guid.NewGuid(), ZoneId = zoneId, ConcertDateId = dateId, Price = 100m }
            ],
            Discounts =
            [
                new Discount { Type = DiscountType.EarlyBird, Percentage = 10m }
            ]
        };
    }

    [Fact]
    public void ToReservationDetailsViewModel_MapsCoreFields()
    {
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var r = BuildReservation(zoneId, dateId);
        var concert = new Concert { Name = "Tour", Venue = "Arena", City = "Beograd" };
        var concertDate = new ConcertDate { Id = dateId, Date = new DateTime(2026, 6, 1), Concert = concert, Tickets = r.Tickets };
        var zone = new Zone { Id = zoneId, Name = "VIP", Price = 100m, Capacity = 100 };

        var vm = r.ToReservationDetailsViewModel(concertDate, new List<Zone> { zone }, r.Tickets.ToList());

        Assert.Equal(r.Id, vm.ReservationId);
        Assert.Equal("Active", vm.Status);
        Assert.Equal("Jovan Jovanović", vm.CustomerName);
        Assert.Equal("j@x.rs", vm.CustomerEmail);
        Assert.Equal("tok-abc", vm.AccessToken);
        Assert.Equal("USE1234567", vm.UsedPromoCode);
        Assert.Equal("GEN1234567", vm.GeneratedPromoCode);
        Assert.False(vm.IsGeneratedPromoCodeUsed);
        Assert.Equal("Tour", vm.ConcertName);
        Assert.Equal("Arena", vm.ConcertVenue);
        Assert.Equal("Beograd", vm.ConcertCity);
    }

    [Fact]
    public void ToReservationDetailsViewModel_ComputesTicketsAndDiscounts()
    {
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var r = BuildReservation(zoneId, dateId);
        var zone = new Zone { Id = zoneId, Name = "VIP", Price = 100m, Capacity = 50 };
        var concertDate = new ConcertDate { Id = dateId, Tickets = r.Tickets };

        var vm = r.ToReservationDetailsViewModel(concertDate, new List<Zone> { zone }, r.Tickets.ToList());

        Assert.Equal(2, vm.Tickets.Count);
        Assert.All(vm.Tickets, t => Assert.Equal("VIP", t.ZoneName));
        Assert.Single(vm.Discounts);
        Assert.Equal("EarlyBird", vm.Discounts[0].Type);
        Assert.Equal(10m, vm.Discounts[0].Percentage);
        // TotalPrice: zone.Price * count = 100 * 2 = 200
        Assert.Equal(200m, vm.TotalPrice);
        // FinalPrice = sum of ticket.Price = 100 + 100 = 200
        Assert.Equal(200m, vm.FinalPrice);
    }

    [Fact]
    public void ToReservationDetailsViewModel_UnknownZone_FallsBackToUnknownLabel()
    {
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var r = BuildReservation(zoneId, dateId);
        var concertDate = new ConcertDate { Id = dateId, Tickets = r.Tickets };

        // No zone matching zoneId provided
        var vm = r.ToReservationDetailsViewModel(concertDate, new List<Zone>(), r.Tickets.ToList());

        Assert.All(vm.Tickets, t => Assert.Equal("Unknown", t.ZoneName));
        // With no matching zone, TotalPrice falls back to 0
        Assert.Equal(0m, vm.TotalPrice);
    }

    [Fact]
    public void ToReservationDetailsViewModel_NullConcertDate_DoesNotThrow()
    {
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var r = BuildReservation(zoneId, dateId);
        var zone = new Zone { Id = zoneId, Name = "VIP", Price = 100m, Capacity = 50 };

        var vm = r.ToReservationDetailsViewModel(null, new List<Zone> { zone }, r.Tickets.ToList());

        Assert.Null(vm.ConcertDate);
        Assert.Null(vm.ConcertName);
        Assert.Equal(200m, vm.TotalPrice);
        Assert.Single(vm.ZonesDetails);
    }

    [Fact]
    public void ToReservationDetailsViewModel_NullGeneratedPromoCode_DoesNotThrow()
    {
        var zoneId = Guid.NewGuid();
        var dateId = Guid.NewGuid();
        var r = BuildReservation(zoneId, dateId);
        r.GeneratedPromoCode = null;
        var zone = new Zone { Id = zoneId, Name = "VIP", Price = 100m, Capacity = 50 };
        var concertDate = new ConcertDate { Id = dateId, Tickets = r.Tickets };

        var vm = r.ToReservationDetailsViewModel(concertDate, new List<Zone> { zone }, r.Tickets.ToList());

        Assert.Null(vm.GeneratedPromoCode);
        Assert.False(vm.IsGeneratedPromoCodeUsed);
    }
}
