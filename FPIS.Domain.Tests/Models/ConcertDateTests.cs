using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class ConcertDateTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new ConcertDate().Id);

    [Fact]
    public void Date_DefaultsToMinValue() => Assert.Equal(default(DateTime), new ConcertDate().Date);

    [Fact]
    public void ConcertId_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new ConcertDate().ConcertId);

    [Fact]
    public void Concert_DefaultsToNonNullEmptyConcert()
    {
        var cd = new ConcertDate();
        Assert.NotNull(cd.Concert);
        Assert.Equal(Guid.Empty, cd.Concert.Id);
    }

    [Fact]
    public void Tickets_DefaultsToNonNullEmptyCollection()
    {
        var cd = new ConcertDate();
        Assert.NotNull(cd.Tickets);
        Assert.Empty(cd.Tickets);
    }

    [Fact]
    public void Date_RoundTrip()
    {
        var when = new DateTime(2026, 6, 15, 20, 30, 0, DateTimeKind.Utc);
        var cd = new ConcertDate { Date = when };
        Assert.Equal(when, cd.Date);
    }

    [Fact]
    public void Date_AcceptsMaxValue()
    {
        var cd = new ConcertDate { Date = DateTime.MaxValue };
        Assert.Equal(DateTime.MaxValue, cd.Date);
    }

    [Fact]
    public void Concert_CanBeReassigned()
    {
        var newConcert = new Concert { Name = "New" };
        var cd = new ConcertDate { Concert = newConcert };
        Assert.Same(newConcert, cd.Concert);
    }

    [Fact]
    public void Tickets_CanAddItems()
    {
        var cd = new ConcertDate();
        var t1 = new ReservationTicket();
        var t2 = new ReservationTicket();
        cd.Tickets.Add(t1);
        cd.Tickets.Add(t2);
        Assert.Equal(2, cd.Tickets.Count);
    }
}
