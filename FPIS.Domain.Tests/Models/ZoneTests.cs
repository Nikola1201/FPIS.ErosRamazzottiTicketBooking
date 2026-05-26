using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class ZoneTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new Zone().Id);

    [Fact]
    public void Name_DefaultsToEmptyString() => Assert.Equal(string.Empty, new Zone().Name);

    [Fact]
    public void Capacity_DefaultsToZero() => Assert.Equal(0, new Zone().Capacity);

    [Fact]
    public void Price_DefaultsToZero() => Assert.Equal(0m, new Zone().Price);

    [Fact]
    public void Capacity_AcceptsZero() => Assert.Equal(0, new Zone { Capacity = 0 }.Capacity);

    [Fact]
    public void Capacity_AcceptsMaxValue() => Assert.Equal(int.MaxValue, new Zone { Capacity = int.MaxValue }.Capacity);

    [Fact]
    public void Capacity_AcceptsNegativeValue()
    {
        // Negative capacity is not validated at the POCO level — invariant check is at service layer.
        var zone = new Zone { Capacity = -1 };
        Assert.Equal(-1, zone.Capacity);
    }

    [Fact]
    public void Price_AcceptsZero() => Assert.Equal(0m, new Zone { Price = 0m }.Price);

    [Fact]
    public void Price_AcceptsMaxValue() => Assert.Equal(decimal.MaxValue, new Zone { Price = decimal.MaxValue }.Price);

    [Fact]
    public void Price_AcceptsFractional() => Assert.Equal(99.99m, new Zone { Price = 99.99m }.Price);

    [Theory]
    [InlineData("VIP")]
    [InlineData("Standing — front")]
    [InlineData("Партер")]
    public void Name_RoundTrip(string input) => Assert.Equal(input, new Zone { Name = input }.Name);
}
