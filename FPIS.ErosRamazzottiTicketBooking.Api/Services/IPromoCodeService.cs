using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Utility;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

/// <summary>Apstrakcija za rad sa promo kodovima (<see cref="PromoCode"/>): primena, validacija i generisanje.</summary>
public interface IPromoCodeService
{
    /// <summary>Označava promo kod kao iskorišćen za datu rezervaciju.</summary>
    /// <param name="reservationId">Identifikator rezervacije.</param>
    /// <param name="promoCode">Promo kod koji se primenjuje (može biti null — tada se ne radi ništa).</param>
    void ApplyPromoCode(Guid reservationId, PromoCode? promoCode);
    /// <summary>Proverava da li je dati promo kod validan i neiskorišćen.</summary>
    /// <param name="promoCode">Tekstualna vrednost promo koda.</param>
    /// <returns>Validan <see cref="PromoCode"/> ili null ako ne postoji ili je već iskorišćen.</returns>
    Task<PromoCode?> IsValidPromoCodeAsync(string promoCode);

    /// <summary>Generiše novi promo kod za rezervaciju (kao nagrada).</summary>
    /// <param name="reservationId">Identifikator rezervacije koja generiše promo kod.</param>
    /// <returns>Novokreirani <see cref="PromoCode"/>.</returns>
    Task<PromoCode> GeneratePromoCode(Guid reservationId);
}

/// <summary>Implementacija <see cref="IPromoCodeService"/> nad <see cref="IUnitOfWork"/>.</summary>
public class PromoCodeService : IPromoCodeService
{
    private readonly ILogger<PromoCodeService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>Konstruktor sa logger-om i <see cref="IUnitOfWork"/>.</summary>
    /// <param name="logger">Logger.</param>
    /// <param name="unitOfWork">Jedinica rada za pristup repozitorijumima.</param>
    public PromoCodeService(ILogger<PromoCodeService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

