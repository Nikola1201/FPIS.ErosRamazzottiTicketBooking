using Microsoft.Extensions.Logging;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.Infrastructure.Repositories;
using FPIS.Domain.Mappings;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface IHomeService
{
    Task<Result<HomePageViewModel>> GetHomePage();
}

public class HomeService : IHomeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HomeService> _logger;

    public HomeService(IUnitOfWork unitOfWork, ILogger<HomeService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(UnitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<HomePageViewModel>> GetHomePage()
    {
        try
        {
            var concertRepo = _unitOfWork.Repository<Concert>();
            var concert = (await concertRepo.GetAllAsync(
               predicate: null,
               asNoTracking: true,
               includes: c=>c.Dates
            )).FirstOrDefault();

            if (concert == null)
            {
                _logger.LogWarning("No concert found when fetching home page data.");
                return Result<HomePageViewModel>.Failure("No concert found.", 404);
            }

            var viewModel = new HomePageViewModel
            {
                Title = "Welcome to Eros Ramazzotti Live!",
                Subtitle = "Experience the concert of a lifetime.",
                ImageUrl = "https://ramazzotti.com/wp-content/uploads/2025/04/er_wt2025_bg.jpg",
                Description = "Book your tickets for the upcoming Eros Ramazzotti concert.",
                ButtonText = "Book Now",
                ButtonUrl = "/reservation",
                Concert = concert.ToViewModel()
            };

            return Result<HomePageViewModel>.Success(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in HomeService.GetHomePage.");
            return Result<HomePageViewModel>.Failure("Internal server error.", 500);
        }
    }
}
