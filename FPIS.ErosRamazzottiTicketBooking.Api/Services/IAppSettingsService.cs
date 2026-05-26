using FPIS.Domain.Models;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface IAppSettingsService
{
    Task<Dictionary<string, string>> GetAppSettingsAsync();
    Task<(
        int earlyBirdDiscountPercent,
        int earlyBirdDaysBefore,
        int fifthTicketDiscountPercent,
        int friendPromoDiscountPercent)> GetDiscountSettings();
}
public class AppSettingsService : IAppSettingsService
{
    private readonly IUnitOfWork _unitOfwork;
    private readonly ILogger<AppSettingsService> _logger;

    public AppSettingsService(IUnitOfWork unitOfWork, ILogger<AppSettingsService> logger)
    {
        _unitOfwork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    }
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
