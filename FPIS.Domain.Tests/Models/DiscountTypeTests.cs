using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class DiscountTypeTests
{
    [Fact]
    public void HasExactlyThreeMembers()
    {
        var names = Enum.GetNames<DiscountType>();
        Assert.Equal(3, names.Length);
    }

    [Fact]
    public void DefinedMembers_ExactSet()
    {
        var names = Enum.GetNames<DiscountType>().OrderBy(n => n).ToArray();
        Assert.Equal(new[] { "EarlyBird", "FifthTicket", "FriendPromo" }, names);
    }

    [Theory]
    [InlineData(DiscountType.EarlyBird, 0)]
    [InlineData(DiscountType.FifthTicket, 1)]
    [InlineData(DiscountType.FriendPromo, 2)]
    public void BackingValues_Stable(DiscountType value, int expected) =>
        Assert.Equal(expected, (int)value);

    [Fact]
    public void IsDefined_KnownValue_True()
    {
        Assert.True(Enum.IsDefined(typeof(DiscountType), DiscountType.EarlyBird));
    }

    [Fact]
    public void IsDefined_SentinelValue_False()
    {
        Assert.False(Enum.IsDefined(typeof(DiscountType), (DiscountType)999));
    }
}
