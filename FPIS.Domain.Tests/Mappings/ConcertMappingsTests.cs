using FPIS.Domain.Mappings;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using Xunit;

namespace FPIS.Domain.Tests.Mappings;

public class ConcertMappingsTests
{
    [Fact]
    public void ToViewModel_CopiesScalarFields()
    {
        var concert = new Concert
        {
            Name = "Tour 2026",
            City = "Beograd",
            Venue = "Štark Arena",
            Address = "Bulevar AČ 58",
            AdditionalInfo = "Doors open at 19:00",
            Dates = new List<ConcertDate>()
        };

        var vm = concert.ToViewModel();

        Assert.Equal("Tour 2026", vm.Title);
        Assert.Equal("Beograd", vm.City);
        Assert.Equal("Štark Arena", vm.Venue);
        Assert.Equal("Bulevar AČ 58", vm.Address);
        Assert.Equal("Doors open at 19:00", vm.AdditionalInfo);
    }

    [Fact]
    public void ToViewModel_MapsDatesToDateTimeList()
    {
        var d1 = new DateTime(2026, 6, 1);
        var d2 = new DateTime(2026, 6, 2);
        var concert = new Concert { Dates = new List<ConcertDate> { new() { Date = d1 }, new() { Date = d2 } } };

        var vm = concert.ToViewModel();

        Assert.NotNull(vm.Dates);
        Assert.Equal(2, vm.Dates!.Count);
        Assert.Contains(d1, vm.Dates);
        Assert.Contains(d2, vm.Dates);
    }

    [Fact]
    public void ToViewModel_WhenDatesIsEmpty_ReturnsEmptyList()
    {
        var concert = new Concert { Dates = new List<ConcertDate>() };
        var vm = concert.ToViewModel();
        Assert.NotNull(vm.Dates);
        Assert.Empty(vm.Dates!);
    }

    [Fact]
    public void ToReservationPageViewModel_ComposesConcertDatesAndAppSettings()
    {
        var dateId = Guid.NewGuid();
        var concertDate = new ConcertDate { Id = dateId, Date = new DateTime(2026, 6, 1) };
        var concert = new Concert
        {
            Name = "Tour 2026",
            Dates = new List<ConcertDate> { concertDate }
        };
        var zones = new List<Zone> { new() { Id = Guid.NewGuid(), Name = "VIP", Capacity = 100, Price = 200m } };
        var tickets = new List<ReservationTicket>();
        var appSettings = new List<AppSettings>
        {
            new() { Key = "MaxTickets", Value = "10" },
            new() { Key = "PromoActive", Value = "true" }
        };

        var vm = concert.ToReservationPageViewModel(zones, tickets, appSettings);

        Assert.Equal("Tour 2026", vm.Concert!.Title);
        Assert.Single(vm.Dates);
        Assert.Equal(dateId, vm.Dates[0].Id);
        Assert.Equal(2, vm.AppSettings.Count);
        Assert.Equal("10", vm.AppSettings["MaxTickets"]);
        Assert.Equal("true", vm.AppSettings["PromoActive"]);
    }

    [Fact]
    public void ToReservationPageViewModel_NullDates_ReturnsEmptyDatesList()
    {
        var concert = new Concert { Name = "Tour 2026", Dates = null! };
        var zones = new List<Zone>();
        var tickets = new List<ReservationTicket>();
        var appSettings = new List<AppSettings>();

        var vm = concert.ToReservationPageViewModel(zones, tickets, appSettings);

        Assert.NotNull(vm.Dates);
        Assert.Empty(vm.Dates);
    }
}
