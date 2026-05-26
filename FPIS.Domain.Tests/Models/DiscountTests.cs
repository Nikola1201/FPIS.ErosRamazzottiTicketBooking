using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class DiscountTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new Discount().Id);

    [Fact]
    public void Type_DefaultsToFirstMember() => Assert.Equal(DiscountType.EarlyBird, new Discount().Type);

    [Fact]
    public void Percentage_DefaultsToZero() => Assert.Equal(0m, new Discount().Percentage);

    [Fact]
    public void ReservationId_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new Discount().ReservationId);

    [Theory]
    [InlineData(0)]
    [InlineData(5.5)]
    [InlineData(100)]
    public void Percentage_RoundTrip(double pct)
    {
        var d = new Discount { Percentage = (decimal)pct };
        Assert.Equal((decimal)pct, d.Percentage);
    }

    [Fact]
    public void Percentage_AcceptsBoundaryValues()
    {
        Assert.Equal(decimal.MinValue, new Discount { Percentage = decimal.MinValue }.Percentage);
        Assert.Equal(decimal.MaxValue, new Discount { Percentage = decimal.MaxValue }.Percentage);
    }

    [Fact]
    public void Reservation_Wiring()
    {
        var r = new Reservation { Id = Guid.NewGuid() };
        var d = new Discount { Reservation = r, ReservationId = r.Id };
        Assert.Same(r, d.Reservation);
        Assert.Equal(r.Id, d.ReservationId);
    }

    [Theory]
    [InlineData(DiscountType.EarlyBird)]
    [InlineData(DiscountType.FifthTicket)]
    [InlineData(DiscountType.FriendPromo)]
    public void Type_RoundTrip(DiscountType type) =>
        Assert.Equal(type, new Discount { Type = type }.Type);
}
