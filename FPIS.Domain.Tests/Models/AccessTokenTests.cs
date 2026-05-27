using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class AccessTokenTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new AccessToken().Id);

    [Fact]
    public void ReservationId_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new AccessToken().ReservationId);

    [Fact]
    public void Value_DefaultsToEmptyString() => Assert.Equal(string.Empty, new AccessToken().Value);

    [Fact]
    public void IsActive_DefaultsToFalse() => Assert.False(new AccessToken().IsActive);

    [Theory]
    [InlineData("")]
    [InlineData("abc123")]
    [InlineData("Имена ћирилицом")]
    public void Value_RoundTrip(string value) => Assert.Equal(value, new AccessToken { Value = value }.Value);

    [Fact]
    public void Reservation_Wiring()
    {
        var r = new Reservation { Id = Guid.NewGuid() };
        var token = new AccessToken { Reservation = r, ReservationId = r.Id };
        Assert.Same(r, token.Reservation);
        Assert.Equal(r.Id, token.ReservationId);
    }

    [Fact]
    public void IsActive_RoundTrip()
    {
        Assert.True(new AccessToken { IsActive = true }.IsActive);
        Assert.False(new AccessToken { IsActive = false }.IsActive);
    }
}
