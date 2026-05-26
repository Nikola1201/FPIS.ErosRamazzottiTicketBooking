using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class PromoCodeTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new PromoCode().Id);

    [Fact]
    public void Code_DefaultsToNonNullDefault()
    {
        var pc = new PromoCode();
        // `Code` is `default!` so it's null but the analyzer is told it isn't.
        // At runtime the value is null.
        Assert.Null(pc.Code);
    }

    [Fact]
    public void IsUsed_DefaultsToFalse() => Assert.False(new PromoCode().IsUsed);

    [Fact]
    public void IsActive_DefaultsToFalse() => Assert.False(new PromoCode().IsActive);

    [Fact]
    public void UsedByReservationId_DefaultsToNull() => Assert.Null(new PromoCode().UsedByReservationId);

    [Fact]
    public void UsedByReservation_DefaultsToNull() => Assert.Null(new PromoCode().UsedByReservation);

    [Fact]
    public void GeneratedByReservationId_DefaultsToNull() => Assert.Null(new PromoCode().GeneratedByReservationId);

    [Fact]
    public void GeneratedByReservation_DefaultsToNull() => Assert.Null(new PromoCode().GeneratedByReservation);

    [Theory]
    [InlineData("1234567890")]
    [InlineData("ABCDEFGHIJ")]
    [InlineData("")]
    public void Code_RoundTrip(string code) => Assert.Equal(code, new PromoCode { Code = code }.Code);

    [Fact]
    public void IsUsed_RoundTrip()
    {
        Assert.True(new PromoCode { IsUsed = true }.IsUsed);
        Assert.False(new PromoCode { IsUsed = false }.IsUsed);
    }

    [Fact]
    public void IsActive_RoundTrip()
    {
        Assert.True(new PromoCode { IsActive = true }.IsActive);
        Assert.False(new PromoCode { IsActive = false }.IsActive);
    }

    [Fact]
    public void UsedByReservation_Wiring()
    {
        var r = new Reservation { Id = Guid.NewGuid() };
        var pc = new PromoCode { UsedByReservation = r, UsedByReservationId = r.Id };
        Assert.Same(r, pc.UsedByReservation);
        Assert.Equal(r.Id, pc.UsedByReservationId);
    }

    [Fact]
    public void GeneratedByReservation_Wiring()
    {
        var r = new Reservation { Id = Guid.NewGuid() };
        var pc = new PromoCode { GeneratedByReservation = r, GeneratedByReservationId = r.Id };
        Assert.Same(r, pc.GeneratedByReservation);
        Assert.Equal(r.Id, pc.GeneratedByReservationId);
    }
}
