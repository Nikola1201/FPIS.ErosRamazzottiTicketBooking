using FPIS.Domain.Models;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

/// <summary>Apstrakcija za rad sa konfiguracionim parametrima aplikacije (<see cref="AppSettings"/>).</summary>
public interface IAppSettingsService
{
    /// <summary>Vraća sve konfiguracione parametre aplikacije kao key/value dictionary.</summary>
    /// <returns>Dictionary key/value parova.</returns>
    Task<Dictionary<string, string>> GetAppSettingsAsync();
    /// <summary>Vraća konfiguraciju popusta: procente i broj dana za Early Bird popust.</summary>
    /// <returns>Tuple sa procentom Early Bird popusta, brojem dana unapred, procentom popusta za petu kartu i procentom promo popusta od prijatelja.</returns>
    Task<(
        int earlyBirdDiscountPercent,
        int earlyBirdDaysBefore,
        int fifthTicketDiscountPercent,
        int friendPromoDiscountPercent)> GetDiscountSettings();
}

/// <summary>Implementacija <see cref="IAppSettingsService"/> nad <see cref="IUnitOfWork"/>.</summary>
public class AppSettingsService : IAppSettingsService
{
    private readonly IUnitOfWork _unitOfwork;
    private readonly ILogger<AppSettingsService> _logger;

    /// <summary>Konstruktor sa <see cref="IUnitOfWork"/> i logger-om.</summary>
    /// <param name="unitOfWork">Jedinica rada za pristup repozitorijumima.</param>
    /// <param name="logger">Logger.</param>
    public AppSettingsService(IUnitOfWork unitOfWork, ILogger<AppSettingsService> logger)
    {
        _unitOfwork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }
    /// <inheritdoc />
    public async Task<Dictionary<string, string>> GetAppSettingsAsync()
    {
        try
        {
            var settingsRepo = _unitOfwork.Repository<AppSettings>();
            var settings = await settingsRepo.GetAllAsync(asNoTracking: true);
            return settings.ToDictionary(s => s.Key, s => s.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in AppSettingsService.GetAppSettings.");
            return new Dictionary<string, string>();
        }
    }

    /// <inheritdoc />
    public Task<(int earlyBirdDiscountPercent, int earlyBirdDaysBefore, int fifthTicketDiscountPercent, int friendPromoDiscountPercent)> GetDiscountSettings()
    {
        return GetAppSettingsAsync().ContinueWith(task =>
        {
            var settings = task.Result;
            int earlyBirdDiscountPercent = int.Parse(settings.GetValueOrDefault("EarlyBirdDiscountPercentage", "0"));
            int earlyBirdDaysBefore = int.Parse(settings.GetValueOrDefault("EarlyBirdDiscountDaysBefore", "0"));
            int fifthTicketDiscountPercent = int.Parse(settings.GetValueOrDefault("FifthTicketDiscountPercentage", "0"));
            int friendPromoDiscountPercent = int.Parse(settings.GetValueOrDefault("FriendPromoDiscountPercentage", "0"));
            return (earlyBirdDiscountPercent, earlyBirdDaysBefore, fifthTicketDiscountPercent, friendPromoDiscountPercent);
        });

    }
}
