using FPIS.Domain.Models;
using Xunit;

namespace FPIS.Domain.Tests.Models;

public class CustomerTests
{
    [Fact]
    public void Id_DefaultsToEmptyGuid() => Assert.Equal(Guid.Empty, new Customer().Id);

    [Theory]
    [InlineData(nameof(Customer.FirstName))]
    [InlineData(nameof(Customer.LastName))]
    [InlineData(nameof(Customer.Email))]
    [InlineData(nameof(Customer.ConfirmedEmail))]
    [InlineData(nameof(Customer.Address))]
    [InlineData(nameof(Customer.City))]
    [InlineData(nameof(Customer.PostalCode))]
    [InlineData(nameof(Customer.Country))]
    public void RequiredStringProperties_DefaultToEmpty(string propertyName)
    {
        var customer = new Customer();
        var value = typeof(Customer).GetProperty(propertyName)!.GetValue(customer);
        Assert.Equal(string.Empty, value);
    }

    [Theory]
    [InlineData(nameof(Customer.Address2))]
    [InlineData(nameof(Customer.PhoneNumber))]
    [InlineData(nameof(Customer.Company))]
    public void OptionalStringProperties_DefaultToNull(string propertyName)
    {
        var customer = new Customer();
        var value = typeof(Customer).GetProperty(propertyName)!.GetValue(customer);
        Assert.Null(value);
    }

    [Fact]
    public void AllStringProperties_RoundTrip()
    {
        var id = Guid.NewGuid();
        var customer = new Customer
        {
            Id = id,
            FirstName = "Jovan",
            LastName = "Jovanović",
            Email = "jovan@example.com",
            ConfirmedEmail = "jovan@example.com",
            Address = "Glavna 1",
            Address2 = "Sprat 3",
            City = "Beograd",
            PostalCode = "11000",
            Country = "Srbija",
            PhoneNumber = "+381601234567",
            Company = "ACME"
        };
        Assert.Equal(id, customer.Id);
        Assert.Equal("Jovan", customer.FirstName);
        Assert.Equal("Jovanović", customer.LastName);
        Assert.Equal("jovan@example.com", customer.Email);
        Assert.Equal("jovan@example.com", customer.ConfirmedEmail);
        Assert.Equal("Glavna 1", customer.Address);
        Assert.Equal("Sprat 3", customer.Address2);
        Assert.Equal("Beograd", customer.City);
        Assert.Equal("11000", customer.PostalCode);
        Assert.Equal("Srbija", customer.Country);
        Assert.Equal("+381601234567", customer.PhoneNumber);
        Assert.Equal("ACME", customer.Company);
    }

    [Fact]
    public void Email_AcceptsUnicode()
    {
        var customer = new Customer { Email = "корисник@домен.рс" };
        Assert.Equal("корисник@домен.рс", customer.Email);
    }

    [Fact]
    public void Address_AcceptsVeryLongString()
    {
        var longString = new string('a', 10_000);
        var customer = new Customer { Address = longString };
        Assert.Equal(longString, customer.Address);
    }
}
