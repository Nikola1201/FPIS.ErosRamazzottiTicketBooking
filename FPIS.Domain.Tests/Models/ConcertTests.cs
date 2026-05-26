using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class ConcertTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid()
    {
        Assert.Equal(Guid.Empty, new Concert().Id);
    }

    [Fact]
    public void Name_DefaultsToEmptyString()
    {
        Assert.Equal(string.Empty, new Concert().Name);
    }

    [Fact]
    public void City_DefaultsToEmptyString() => Assert.Equal(string.Empty, new Concert().City);

    [Fact]
    public void Venue_DefaultsToEmptyString() => Assert.Equal(string.Empty, new Concert().Venue);

    [Fact]
    public void Address_DefaultsToEmptyString() => Assert.Equal(string.Empty, new Concert().Address);

    [Fact]
    public void AdditionalInfo_DefaultsToEmptyString() => Assert.Equal(string.Empty, new Concert().AdditionalInfo);

    [Fact]
    public void Dates_DefaultsToNonNullEmptyCollection()
    {
        var concert = new Concert();
        Assert.NotNull(concert.Dates);
        Assert.Empty(concert.Dates);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Standard name")]
    [InlineData("Eros Ramazzotti — Live!")]
    [InlineData("Имена кирилицом")]
    public void Name_RoundTrip(string input)
    {
        var concert = new Concert { Name = input };
        Assert.Equal(input, concert.Name);
    }

    [Fact]
    public void Name_AcceptsVeryLongString()
    {
        var longString = new string('x', 10_000);
        var concert = new Concert { Name = longString };
        Assert.Equal(longString, concert.Name);
    }

    [Fact]
    public void Id_RoundTrip()
    {
        var id = Guid.NewGuid();
        var concert = new Concert { Id = id };
        Assert.Equal(id, concert.Id);
    }

    [Fact]
    public void Dates_CanBeAssignedAndRead()
    {
        var date = new ConcertDate();
        var concert = new Concert();
        concert.Dates.Add(date);
        Assert.Single(concert.Dates);
        Assert.Same(date, concert.Dates.First());
    }
}
