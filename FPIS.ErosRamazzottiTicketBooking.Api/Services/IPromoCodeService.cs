using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Utility;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;
public interface IPromoCodeService
{
    void ApplyPromoCode(Guid reservationId, PromoCode? promoCode);
    Task<PromoCode?> IsValidPromoCodeAsync(string promoCode);

    Task<PromoCode> GeneratePromoCode(Guid reservationId);
}

public class PromoCodeService : IPromoCodeService
{
    private readonly ILogger<PromoCodeService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public PromoCodeService(ILogger<PromoCodeService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public void ApplyPromoCode(Guid reservationId, PromoCode? promoCode)
    {
        if (promoCode == null)
            return;
        try
        {
            var promoCodeRepo = _unitOfWork.Repository<PromoCode>();
            promoCode.IsUsed = true;
            promoCode.UsedByReservationId = reservationId;
            promoCodeRepo.Update(promoCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in PromoCodeService.ApplyPromoCode.");
        }
    }

    public async Task<PromoCode> GeneratePromoCode(Guid reservationId)
    {
        try
        {
            var promoCodeRepo = _unitOfWork.Repository<PromoCode>();
            var newPromoCode = new PromoCode
            {
                Id = Guid.NewGuid(),
                Code = TokenGenerator.GenerateTokenValue(10),
                IsUsed = false,
                GeneratedByReservationId = reservationId
            };
            await promoCodeRepo.AddAsync(newPromoCode);
            return newPromoCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in PromoCodeService.GeneratePromoCode.");
            return new();
        }
    }

    public async Task<PromoCode?> IsValidPromoCodeAsync(string promoCode)
    {
        try
        {
            var promoCodeRepo = _unitOfWork.Repository<PromoCode>();
            var existingPromoCode = (await promoCodeRepo.GetAllAsync(
                pc => pc.Code == promoCode && !pc.IsUsed, asNoTracking: true)).FirstOrDefault();
            return existingPromoCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in PromoCodeService.IsValidPromoCodeAsync.");
            return null;
        }
    }
}

