using FPIS.Domain.Models;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services
{
    public interface IZoneService
    {
        Task<Dictionary<Guid, Zone>> GetAllZones();
    }
    public class ZoneService : IZoneService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ZoneService> _logger;
        public ZoneService(IUnitOfWork unitOfWork, ILogger<ZoneService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
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
