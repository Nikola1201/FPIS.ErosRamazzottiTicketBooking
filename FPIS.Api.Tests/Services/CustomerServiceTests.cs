using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class CustomerServiceTests
{
    private static (CustomerService svc, Mock<IUnitOfWork> uow, Mock<IRepository<Customer>> repo) Build()
    {
        var repo = new Mock<IRepository<Customer>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<Customer>()).Returns(repo.Object);
        var logger = Mock.Of<ILogger<CustomerService>>();
        return (new CustomerService(logger, uow.Object), uow, repo);
    }

    private static CustomerCreateDTO MakeDto(string email = "a@b.rs") => new()
    {
        FirstName = "A",
        LastName = "B",
        Email = email,
        ConfirmedEmail = email,
        Address = "X",
        City = "Y",
        PostalCode = "Z",
        Country = "RS"
    };

    [Fact]
    public void CreateCustomer_WhenEmailUnique_ReturnsNewCustomerWithGuidId()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Customer, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync(new List<Customer>());

        var result = svc.CreateCustomer(MakeDto("new@x.rs"));

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result!.Id);
        Assert.Equal("new@x.rs", result.Email);
        repo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public void CreateCustomer_WhenEmailExists_ReturnsNull()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Customer, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync(new List<Customer> { new() { Email = "exists@x.rs" } });

        var result = svc.CreateCustomer(MakeDto("exists@x.rs"));

        Assert.Null(result);
        repo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public void CreateCustomer_WhenRepoThrows_ReturnsNull()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Customer, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("boom"));

        var result = svc.CreateCustomer(MakeDto());

        Assert.Null(result);
    }

    [Fact]
    public void CreateCustomer_CopiesAllFieldsFromDto()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Customer, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<Customer, object>>[]>()))
            .ReturnsAsync(new List<Customer>());
        var dto = new CustomerCreateDTO
        {
            FirstName = "Jovan",
            LastName = "Jovanović",
            Email = "j@x.rs",
            ConfirmedEmail = "j@x.rs",
            PhoneNumber = "+381601234567",
            Address = "Glavna 1",
            Address2 = "Sprat 3",
            City = "Beograd",
            PostalCode = "11000",
            Country = "Srbija",
            Company = "ACME"
        };

        var result = svc.CreateCustomer(dto);

        Assert.NotNull(result);
        Assert.Equal("Jovan", result!.FirstName);
        Assert.Equal("Jovanović", result.LastName);
        Assert.Equal("Glavna 1", result.Address);
        Assert.Equal("Sprat 3", result.Address2);
        Assert.Equal("ACME", result.Company);
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new CustomerService(null!, Mock.Of<IUnitOfWork>()));
    }

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new CustomerService(Mock.Of<ILogger<CustomerService>>(), null!));
    }
}
