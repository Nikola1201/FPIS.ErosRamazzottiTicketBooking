using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class ReservationStatusTests
{
    [Fact]
    public void HasExactlyThreeMembers() => Assert.Equal(3, Enum.GetNames<ReservationStatus>().Length);

    [Fact]
    public void DefinedMembers_ExactSet()
    {
        var names = Enum.GetNames<ReservationStatus>().OrderBy(n => n).ToArray();
        Assert.Equal(new[] { "Active", "Cancelled", "Modified" }, names);
    }

    [Theory]
    [InlineData(ReservationStatus.Active, 0)]
    [InlineData(ReservationStatus.Modified, 1)]
    [InlineData(ReservationStatus.Cancelled, 2)]
    public void BackingValues_Stable(ReservationStatus value, int expected) =>
        Assert.Equal(expected, (int)value);

    [Fact]
    public void IsDefined_KnownValue_True() => Assert.True(Enum.IsDefined(typeof(ReservationStatus), ReservationStatus.Active));

    [Fact]
    public void IsDefined_SentinelValue_False() => Assert.False(Enum.IsDefined(typeof(ReservationStatus), (ReservationStatus)999));
}
