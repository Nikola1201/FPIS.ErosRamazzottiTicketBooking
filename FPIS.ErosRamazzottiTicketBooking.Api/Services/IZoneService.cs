using FPIS.Domain.Models;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services
{
    /// <summary>Apstrakcija za rad sa zonama (<see cref="Zone"/>).</summary>
    public interface IZoneService
    {
        /// <summary>Vraća sve zone u sistemu kao mapu Id → <see cref="Zone"/>.</summary>
        /// <returns>Dictionary identifikatora zone i odgovarajućeg entiteta.</returns>
        Task<Dictionary<Guid, Zone>> GetAllZones();
    }

    /// <summary>Implementacija <see cref="IZoneService"/> nad <see cref="IUnitOfWork"/>.</summary>
    public class ZoneService : IZoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ZoneService> _logger;
        /// <summary>Konstruktor sa <see cref="IUnitOfWork"/> i logger-om.</summary>
        /// <param name="unitOfWork">Jedinica rada za pristup repozitorijumima.</param>
        /// <param name="logger">Logger.</param>
        public ZoneService(IUnitOfWork unitOfWork, ILogger<ZoneService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <inheritdoc />
        public async Task<Dictionary<Guid, Zone>> GetAllZones()
        {
            try
            {
                var zoneRepo = _unitOfWork.Repository<Zone>();
                var zones = (await zoneRepo.GetAllAsync(asNoTracking:true)).ToDictionary(z => z.Id, z => z);
                return zones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in ZoneService.GetAllZones.");
                return [];
            }
        }
    }
}
