using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

/// <summary>Apstrakcija za rad sa kupcima (<see cref="Customer"/>).</summary>
public interface ICustomerService
{
    /// <summary>Kreira novog kupca iz DTO-a; vraća null ako kupac sa istim email-om već postoji.</summary>
    /// <param name="customer">DTO sa podacima o kupcu.</param>
    /// <returns>Novokreirani <see cref="Customer"/> ili null.</returns>
    Task<Customer?> CreateCustomerAsync(CustomerCreateDTO customer);
}

/// <summary>Implementacija <see cref="ICustomerService"/> nad <see cref="IUnitOfWork"/>.</summary>
public class CustomerService : ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>Konstruktor sa logger-om i <see cref="IUnitOfWork"/>.</summary>
    /// <param name="logger">Logger.</param>
    /// <param name="unitOfWork">Jedinica rada za pristup repozitorijumima.</param>
    public CustomerService(ILogger<CustomerService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    /// <inheritdoc />
    public async Task<Customer?> CreateCustomerAsync(CustomerCreateDTO customer)
    {
        try
        {
            var customerRepo = _unitOfWork.Repository<Customer>();
            var existing = await customerRepo.GetAllAsync(c => c.Email == customer.Email, asNoTracking: true);
            if (existing.FirstOrDefault() != null)
            {
                _logger.LogInformation("Customer with email {Email} already exists.", customer.Email);
                return null;
            }
            var newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                ConfirmedEmail = customer.ConfirmedEmail,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                Address2 = customer.Address2,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Country = customer.Country,
                Company = customer.Company,
            };
            await customerRepo.AddAsync(newCustomer);
            return newCustomer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in CustomerService.CreateCustomerAsync.");
            return default;
        }

    }

}
