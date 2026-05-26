using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class ReservationTicketTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new ReservationTicket().Id);

    [Fact]
    public void ReservationId_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new ReservationTicket().ReservationId);

    [Fact]
    public void ZoneId_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new ReservationTicket().ZoneId);

    [Fact]
    public void ConcertDateId_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new ReservationTicket().ConcertDateId);

    [Fact]
    public void Price_DefaultsToZero() => Assert.Equal(0m, new ReservationTicket().Price);

    [Fact]
    public void Price_RoundTrip() => Assert.Equal(123.45m, new ReservationTicket { Price = 123.45m }.Price);

    [Fact]
    public void Price_AcceptsZeroAndMaxValue()
    {
        Assert.Equal(0m, new ReservationTicket { Price = 0m }.Price);
        Assert.Equal(decimal.MaxValue, new ReservationTicket { Price = decimal.MaxValue }.Price);
    }

    [Fact]
    public void Zone_Wiring()
    {
        var zone = new Zone { Id = Guid.NewGuid(), Name = "VIP" };
        var t = new ReservationTicket { Zone = zone, ZoneId = zone.Id };
        Assert.Same(zone, t.Zone);
        Assert.Equal(zone.Id, t.ZoneId);
    }
}
