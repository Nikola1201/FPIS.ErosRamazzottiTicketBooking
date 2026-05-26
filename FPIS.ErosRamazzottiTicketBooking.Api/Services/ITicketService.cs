using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface ITicketService
{
    Task<(bool IsValid, string? Error)> ValidateZoneCapacitiesAsync(
        IEnumerable<TicketRequest> tickets,
        Guid concertDateId,
        IDictionary<Guid, Zone> zones,
        Guid? currentReservationId = null);

    Task<(List<ReservationTicket> Tickets, List<Discount> Discounts)> GenerateTicketsAsync(
        IEnumerable<TicketRequest> tickets,
        Guid concertDateId,
        IDictionary<Guid, Zone> zones,
        ConcertDate concertDate,
        PromoCode? promoCode);
    Task<List<ReservationTicket>> GetTicketsByConcertDate(Guid concertDateId);
    Task<List<ReservationTicket>> GetTicketsByConcertDates(List<Guid> concertDateId);

}
public class TicketService : ITicketService
{
    private readonly ILogger<TicketService> _logger;
    private readonly IAppSettingsService _appSettingsService;
    private readonly IUnitOfWork _unitOfWork;

    public TicketService(IUnitOfWork unitOfWork, ILogger<TicketService> logger, IAppSettingsService appSettingsService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
    }

public async Task<(bool IsValid, string? Error)> ValidateZoneCapacitiesAsync(
    IEnumerable<TicketRequest> tickets,
    Guid concertDateId,
    IDictionary<Guid, Zone> zones,
    Guid? currentReservationId = null)
{
    var ticketRepo = _unitOfWork.Repository<ReservationTicket>();
    
    // Group tickets by zone for batch processing
    var zoneGroups = tickets.GroupBy(t => t.ZoneId);
    
    foreach (var zoneGroup in zoneGroups)
    {
        var zoneId = zoneGroup.Key;
        var totalRequestedQuantity = zoneGroup.Sum(t => t.Quantity);
        
        if (!zones.TryGetValue(zoneId, out var zone))
            return (false, $"Zone {zoneId} not found.");

        // Get reserved count in one query
        var reservedCount = await ticketRepo.CountAsync(
            rt => rt.ConcertDateId == concertDateId && 
                  rt.ZoneId == zoneId && 
                  rt.ReservationId != currentReservationId);

        if (reservedCount + totalRequestedQuantity > zone.Capacity)
            return (false, $"Not enough capacity in zone {zone.Name}. Requested: {totalRequestedQuantity}, Available: {zone.Capacity - reservedCount}");
    }
    
    return (true, null);
}

    public async Task<(List<ReservationTicket> Tickets, List<Discount> Discounts)> GenerateTicketsAsync(
        IEnumerable<TicketRequest> tickets,
        Guid concertDateId,
        IDictionary<Guid, Zone> zones,
        ConcertDate concertDate,
        PromoCode? promoCode)
    {
        var (earlyBirdDiscountPercent, earlyBirdDaysBefore, fifthTicketDiscountPercent, friendPromoDiscountPercent)
       = await _appSettingsService.GetDiscountSettings();
        var resultTickets = new List<ReservationTicket>();
        var discounts = new List<Discount>();
        int ticketCounter = 0;
        bool earlyBird = false, fifthTicket = false, friendPromo = false;

        bool isEarlyBird = (concertDate.Date - DateTime.UtcNow).TotalDays >= earlyBirdDaysBefore;

        foreach (var ticket in tickets)
        {
            var zone = zones[ticket.ZoneId];
            for (int i = 0; i < ticket.Quantity; i++)
            {
                decimal price = zone.Price;
                decimal discount = 0m;

                if (isEarlyBird && earlyBirdDiscountPercent > 0)
                {
                    discount += price * earlyBirdDiscountPercent / 100m;
                    earlyBird = true;
                }
                if (fifthTicketDiscountPercent > 0 && ((ticketCounter + 1) % 5 == 0))
                {
                    discount += price * fifthTicketDiscountPercent / 100m;
                    fifthTicket = true;
                }
                if (promoCode != null && friendPromoDiscountPercent > 0)
                {
                    discount += price * friendPromoDiscountPercent / 100m;
                    friendPromo = true;
                }

                price -= discount;

                resultTickets.Add(new ReservationTicket
                {
                    Id = Guid.NewGuid(),
                    ZoneId = ticket.ZoneId,
                    ConcertDateId = concertDateId,
                    Price = price
                });

                ticketCounter++;
            }
        }

        if (earlyBird)
            discounts.Add(new Discount { Id = Guid.NewGuid(), Type = DiscountType.EarlyBird, Percentage = earlyBirdDiscountPercent });
        if (fifthTicket)
            discounts.Add(new Discount { Id = Guid.NewGuid(), Type = DiscountType.FifthTicket, Percentage = fifthTicketDiscountPercent });
        if (friendPromo)
            discounts.Add(new Discount { Id = Guid.NewGuid(), Type = DiscountType.FriendPromo, Percentage = friendPromoDiscountPercent });

        return (resultTickets, discounts);
    }

    public async Task<List<ReservationTicket>> GetTicketsByConcertDate(Guid concertDateId)
    {
        var ticketRepo = _unitOfWork.Repository<ReservationTicket>();
        var tickets = await ticketRepo.GetAllAsync(
            rt => rt.ConcertDateId == concertDateId,
            asNoTracking: true,
            includes: rt => rt.Zone);
        return tickets.ToList();
    }

    public Task<List<ReservationTicket>> GetTicketsByConcertDates(List<Guid> concertDateIds)
    {
        var ticketRepo = _unitOfWork.Repository<ReservationTicket>();
        return ticketRepo.GetAllAsync(
            rt => concertDateIds.Contains(rt.ConcertDateId),
            asNoTracking: true,
            includes: rt => rt.Zone).ContinueWith(t => t.Result.ToList());

    }
}
