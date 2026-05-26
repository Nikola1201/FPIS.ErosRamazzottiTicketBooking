using FPIS.Domain.Mappings;
using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Mappings;

public class ConcertDateMappingsTests
{
    [Fact]
    public void ToViewModel_MapsScalarFields()
    {
        var id = Guid.NewGuid();
        var when = new DateTime(2026, 6, 15);
        var cd = new ConcertDate { Id = id, Date = when };

        var vm = cd.ToViewModel(zones: new List<Zone>(), tickets: new List<ReservationTicket>());

        Assert.Equal(id, vm.Id);
        Assert.Equal(when, vm.Date);
        Assert.NotNull(vm.Zones);
        Assert.Empty(vm.Zones);
    }

    [Fact]
    public void ToViewModel_ComputesCapacityRemaining_FromReservedTickets()
    {
        var dateId = Guid.NewGuid();
        var zoneId = Guid.NewGuid();
        var cd = new ConcertDate { Id = dateId };
        var zone = new Zone { Id = zoneId, Name = "VIP", Capacity = 100, Price = 50m };
        var tickets = new List<ReservationTicket>
        {
            new() { ConcertDateId = dateId, ZoneId = zoneId },
            new() { ConcertDateId = dateId, ZoneId = zoneId },
            new() { ConcertDateId = dateId, ZoneId = zoneId }
        };

        var vm = cd.ToViewModel(new[] { zone }, tickets);

        Assert.Single(vm.Zones);
        Assert.Equal(100 - 3, vm.Zones[0].CapacityRemaining);
    }

    [Fact]
    public void ToViewModel_IgnoresTicketsForDifferentDate()
    {
        var dateId = Guid.NewGuid();
        var zoneId = Guid.NewGuid();
        var cd = new ConcertDate { Id = dateId };
        var zone = new Zone { Id = zoneId, Capacity = 50 };
        var tickets = new List<ReservationTicket>
        {
            new() { ConcertDateId = Guid.NewGuid(), ZoneId = zoneId } // different date
        };

        var vm = cd.ToViewModel(new[] { zone }, tickets);

        Assert.Equal(50, vm.Zones[0].CapacityRemaining);
    }
}
