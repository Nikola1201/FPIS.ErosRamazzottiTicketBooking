using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Utility;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface ITokenService
{
    Task<AccessToken> CreateToken(Guid reservationId);
}
public class TokenService : ITokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TokenService> _logger;
    public TokenService(IUnitOfWork unitOfWork, ILogger<TokenService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

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

