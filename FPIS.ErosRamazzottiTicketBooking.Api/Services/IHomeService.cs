using Microsoft.Extensions.Logging;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.Infrastructure.Repositories;
using FPIS.Domain.Mappings;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

/// <summary>Apstrakcija za dohvatanje podataka home page-a.</summary>
public interface IHomeService
{
    /// <summary>Vraća view model sa podacima za home page (trenutni koncert i CTA).</summary>
    /// <returns><see cref="Result{T}"/> sa <see cref="HomePageViewModel"/> — uspeh ili greška sa kodom.</returns>
    Task<Result<HomePageViewModel>> GetHomePage();
}

/// <summary>Implementacija <see cref="IHomeService"/> koja koristi <see cref="IUnitOfWork"/> za pristup podacima.</summary>
public class HomeService : IHomeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HomeService> _logger;

    /// <summary>Konstruktor sa <see cref="IUnitOfWork"/> i logger-om.</summary>
    /// <param name="unitOfWork">Jedinica rada za pristup repozitorijumima.</param>
    /// <param name="logger">Logger.</param>
    public HomeService(IUnitOfWork unitOfWork, ILogger<HomeService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(UnitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
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
