using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Utility;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

/// <summary>Apstrakcija za generisanje pristupnih tokena za rezervacije.</summary>
public interface ITokenService
{
    /// <summary>Generiše i čuva novi <see cref="AccessToken"/> za datu rezervaciju.</summary>
    /// <param name="reservationId">Identifikator rezervacije.</param>
    /// <returns>Novokreirani <see cref="AccessToken"/>.</returns>
    Task<AccessToken> CreateToken(Guid reservationId);
}

/// <summary>Implementacija <see cref="ITokenService"/> koja koristi <see cref="TokenGenerator"/> za generisanje vrednosti tokena.</summary>
public class TokenService : ITokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TokenService> _logger;
    /// <summary>Konstruktor sa <see cref="IUnitOfWork"/> i logger-om.</summary>
    /// <param name="unitOfWork">Jedinica rada za pristup repozitorijumima.</param>
    /// <param name="logger">Logger.</param>
    public TokenService(IUnitOfWork unitOfWork, ILogger<TokenService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<AccessToken> CreateToken(Guid reservationId)
    {
        try
        {
            var token = new AccessToken
            {
                Id = Guid.NewGuid(),
                Value = TokenGenerator.GenerateTokenValue(10),
                IsActive = true,
                ReservationId = reservationId
            };

            var tokenRepo = _unitOfWork.Repository<AccessToken>();
            await tokenRepo.AddAsync(token);

            _logger.LogInformation("Created new token with ID: {TokenId}", token.Id);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create token.");
            throw;
        }
    }

}

