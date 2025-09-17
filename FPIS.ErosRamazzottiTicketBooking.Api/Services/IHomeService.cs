using Microsoft.Extensions.Logging;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.Infrastructure.Repositories;
using FPIS.Domain.Mappings;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface IHomeService
{
    Task<HomePageViewModel?> GetHomePage();
}

public class HomeService : IHomeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HomeService> _logger;

    public HomeService(IUnitOfWork unitOfWork, ILogger<HomeService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<HomePageViewModel?> GetHomePage()
    {
        try
        {
            var concertRepo = _unitOfWork.Repository<Concert>();
            var concert = (await concertRepo
                .GetAllAsync(c => c.Dates))       
                .FirstOrDefault();

            if (concert == null)
            {
                _logger.LogWarning("No concert found when fetching home page data.");
                return null;
            }

            return new HomePageViewModel
            {
                Title = "Welcome to Eros Ramazzotti Live!",
                Subtitle = "Experience the concert of a lifetime.",
                ImageUrl = "/images/eros.jpg",
                Description = "Book your tickets for the upcoming Eros Ramazzotti concert.",
                ButtonText = "Book Now",
                ButtonUrl = "/tickets",
                Concert = concert.ToViewModel()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in HomeService.GetHomePage.");
            throw; // Let the controller handle the exception and return ProblemDetails
        }
    }
}
