using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class ReservationTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new Reservation().Id);

    [Fact]
    public void Tickets_DefaultsToNonNullEmptyCollection()
    {
        var r = new Reservation();
        Assert.NotNull(r.Tickets);
        Assert.Empty(r.Tickets);
    }

    [Fact]
    public void Discounts_DefaultsToNonNullEmptyCollection()
    {
        var r = new Reservation();
        Assert.NotNull(r.Discounts);
        Assert.Empty(r.Discounts);
    }

    [Fact]
    public void UsedPromoCode_DefaultsToNull() => Assert.Null(new Reservation().UsedPromoCode);

    [Fact]
    public void UsedPromoCodeId_DefaultsToNull() => Assert.Null(new Reservation().UsedPromoCodeId);

    [Fact]
    public void GeneratedPromoCodeId_DefaultsToNull() => Assert.Null(new Reservation().GeneratedPromoCodeId);

    [Fact]
    public void Status_DefaultsToActive()
    {
        // First-defined enum member is the default for value types
        Assert.Equal(ReservationStatus.Active, new Reservation().Status);
    }

    [Fact]
    public void Customer_RoundTrip()
    {
        var customer = new Customer { FirstName = "Ana" };
        var r = new Reservation { Customer = customer };
        Assert.Same(customer, r.Customer);
    }

    [Fact]
    public void AccessToken_RoundTrip()
    {
        var token = new AccessToken { Value = "abc" };
        var r = new Reservation { AccessToken = token };
        Assert.Same(token, r.AccessToken);
    }

    [Fact]
    public void Tickets_CanAddMultiple()
    {
        var r = new Reservation();
        r.Tickets.Add(new ReservationTicket());
        r.Tickets.Add(new ReservationTicket());
        Assert.Equal(2, r.Tickets.Count);
    }

    [Fact]
    public void UsedPromoCode_NullableWiring()
    {
        var r = new Reservation();
        Assert.Null(r.UsedPromoCode);
        Assert.Null(r.UsedPromoCodeId);

        var pc = new PromoCode { Id = Guid.NewGuid(), Code = "1234567890" };
        r.UsedPromoCode = pc;
        r.UsedPromoCodeId = pc.Id;

        Assert.Same(pc, r.UsedPromoCode);
        Assert.Equal(pc.Id, r.UsedPromoCodeId);
    }

    [Fact]
    public void GeneratedPromoCode_Wiring()
    {
        var pc = new PromoCode { Id = Guid.NewGuid(), Code = "abcdefghij" };
        var r = new Reservation { GeneratedPromoCode = pc, GeneratedPromoCodeId = pc.Id };
        Assert.Same(pc, r.GeneratedPromoCode);
        Assert.Equal(pc.Id, r.GeneratedPromoCodeId);
    }

    [Theory]
    [InlineData(ReservationStatus.Active)]
    [InlineData(ReservationStatus.Modified)]
    [InlineData(ReservationStatus.Cancelled)]
    public void Status_RoundTrip(ReservationStatus status) =>
        Assert.Equal(status, new Reservation { Status = status }.Status);
}
