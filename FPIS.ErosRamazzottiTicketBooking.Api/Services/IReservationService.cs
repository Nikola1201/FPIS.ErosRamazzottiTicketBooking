using FPIS.Domain.Mappings;
using FPIS.Domain.Models;
using FPIS.Domain.ViewModels;
using FPIS.Infrastructure.Repositories;

namespace FPIS.ErosRamazzottiTicketBooking.Api.Services;

public interface IReservationService
{
    Task<Result<ReservationPageViewModel>> GetReservationPage();
    Task<Result<ReservationResultDTO>> CreateReservationAsync(ReservationPostDTO payload);
    Task<Result<ReservationUpdateResultDTO>> UpdateReservationAsync(ReservationUpdateDTO payload);
    Task <Result<ReservationCancelResultDTO>> CancelReservationAsync(Guid reservationId, string customerEmail, string accessToken);
    public Task<Result<ReservationDetailsViewModel>> GetReservationDetails(string accessToken, string customerEmail);

}

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReservationService> _logger;
    private readonly ICustomerService _customerService;
    private readonly IPromoCodeService _promoCodeService;
    private readonly ITicketService _ticketService;
    private readonly ITokenService _tokenService;
    private readonly IZoneService _zoneService;

    public ReservationService(
        IUnitOfWork unitOfWork,
        ILogger<ReservationService> logger,
        ICustomerService customerService,
        IPromoCodeService promoCodeService,
        ITicketService ticketService,
        ITokenService tokenService,
        IZoneService zoneService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        _promoCodeService = promoCodeService ?? throw new ArgumentNullException(nameof(promoCodeService));
        _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _zoneService = zoneService ?? throw new ArgumentNullException(nameof(zoneService));
    }

    public async Task<Result<ReservationCancelResultDTO>> CancelReservationAsync(Guid reservationId, string customerEmail, string accessToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Load the reservation with all necessary includes
            var reservationRepo = _unitOfWork.Repository<Reservation>();
            var reservations = await reservationRepo.GetAllAsync(
                r => r.Id == reservationId &&
                     r.Customer.Email == customerEmail &&
                     r.AccessToken.Value == accessToken,
                false,
                [
                    r => r.Customer,
                r => r.AccessToken,
                r => r.Tickets,
                r => r.Discounts,
                r => r.UsedPromoCode,
                r => r.GeneratedPromoCode
                ]
            );
            var reservation = reservations.FirstOrDefault();

            if (reservation == null)
                return Result<ReservationCancelResultDTO>.Failure("Reservation not found or invalid access token.", 404);

            // Mark reservation as cancelled (optional: or delete)
            reservation.Status = ReservationStatus.Cancelled;

            // Handle generated promo code: check if it was used by another reservation
            var generatedPromo = reservation.GeneratedPromoCode;
            if (generatedPromo != null && generatedPromo.IsUsed && generatedPromo.UsedByReservationId.HasValue)
            {
                // Find the reservation that used this promo code
                var usedByReservationRepo = _unitOfWork.Repository<Reservation>();
                var usedByReservations = await usedByReservationRepo.GetAllAsync(
                    r => r.Id == generatedPromo.UsedByReservationId.Value,
                    false,
                    [
                        r => r.Tickets,
                    r => r.Discounts,
                    r => r.UsedPromoCode
                    ]
                );
                var usedByReservation = usedByReservations.FirstOrDefault();
                if (usedByReservation != null)
                {
                    // Remove old discounts
                    var discountRepo = _unitOfWork.Repository<Discount>();
                    discountRepo.RemoveRange(usedByReservation.Discounts);
                    usedByReservation.Discounts.Clear();

                    // Recalculate discounts without the friend promo
                    var concertDateId = usedByReservation.Tickets.FirstOrDefault()?.ConcertDateId ?? Guid.Empty;
                    var concertDateRepo = _unitOfWork.Repository<ConcertDate>();
                    var concertDate = await concertDateRepo.GetByIdAsync(concertDateId);
                    usedByReservation.UsedPromoCode = null;
                    var zoneServiceZones = await _zoneService.GetAllZones();
                    var (newTickets, newDiscounts) = await _ticketService.GenerateTicketsAsync(
                        usedByReservation.Tickets
                            .GroupBy(t => t.ZoneId)
                            .Select(g => new TicketRequest { ZoneId = g.Key, Quantity = g.Count() }),
                        concertDateId,
                        zoneServiceZones,
                        concertDate,
                        usedByReservation.UsedPromoCode // This will now be null or not include the friend promo
                    );

                    // Update discounts
                    foreach (var discount in newDiscounts)
                    {
                        discount.ReservationId = usedByReservation.Id;
                        usedByReservation.Discounts.Add(discount);
                        await discountRepo.AddAsync(discount);
                    }
                    // Update tickets
                    var ticketRepo = _unitOfWork.Repository<ReservationTicket>();
                    ticketRepo.RemoveRange(usedByReservation.Tickets);
                    usedByReservation.Tickets.Clear();
                    foreach (var ticket in newTickets)
                    {
                        ticket.ReservationId = usedByReservation.Id;
                        usedByReservation.Tickets.Add(ticket);
                        await ticketRepo.AddAsync(ticket);
                    }
                    usedByReservationRepo.Update(usedByReservation);
                }
            }
            var promoCodeRepo = _unitOfWork.Repository<PromoCode>();
            promoCodeRepo.Delete(generatedPromo);
            // Revert used promo code if any
            var usedPromoCode = reservation.UsedPromoCode;
            if(usedPromoCode != null)
            {
                usedPromoCode.IsUsed = false;
                usedPromoCode.UsedByReservationId = null;
                promoCodeRepo = _unitOfWork.Repository<PromoCode>();
                promoCodeRepo.Update(usedPromoCode);
            }


            var customerRepo = _unitOfWork.Repository<Customer>();
            customerRepo.Delete(reservation.Customer);
            // Delete the reservation (cascade deletes tickets/discounts)
            reservationRepo.Delete(reservation);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            var resultDto = new ReservationCancelResultDTO
            {
                ReservationId = reservationId,
                Cancelled = true,
                Message = "Reservation cancelled successfully."
            };

            return Result<ReservationCancelResultDTO>.Success(resultDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "An error occurred while cancelling reservation.");
            return Result<ReservationCancelResultDTO>.Failure("Internal server error.", 500);
        }
    }

    public async Task<Result<ReservationResultDTO>> CreateReservationAsync(ReservationPostDTO payload)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Validate concert date exists
            var concertDateRepo = _unitOfWork.Repository<ConcertDate>();
            var concertDate = await concertDateRepo.GetByIdAsync(payload.ConcertDateId);
            if (concertDate is null)
                return Result<ReservationResultDTO>.Failure("Concert date not found.", 404);

            // Get all zones
            var zones = await _zoneService.GetAllZones();
            if (zones.Count == 0)
                return Result<ReservationResultDTO>.Failure("No zones available.", 400);

            // Validate zones and capacity
            var (isValid, error) = await _ticketService.ValidateZoneCapacitiesAsync(
                payload.Tickets, payload.ConcertDateId, zones);
            if (!isValid)
                return Result<ReservationResultDTO>.Failure(error ?? "Invalid ticket request.", 400);

            // Promo code logic
            PromoCode? promoCode = null;
            if (!string.IsNullOrWhiteSpace(payload.PromoCode))
            {
                promoCode = await _promoCodeService.IsValidPromoCodeAsync(payload.PromoCode);
                if (promoCode is null)
                    return Result<ReservationResultDTO>.Failure("Promo code does not exist or has already been used.", 400);
            }

            // Create customer
            var customer = _customerService.CreateCustomer(payload.Customer);
            if (customer is null)
                return Result<ReservationResultDTO>.Failure("Customer with the same email already exists.", 400);

            // Generate tickets and discounts
            var (allTickets, discounts) = await _ticketService.GenerateTicketsAsync(
                payload.Tickets, payload.ConcertDateId, zones, concertDate, promoCode);

            // Create reservation
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                Customer = customer,
                Status = ReservationStatus.Active,
                Tickets = allTickets,
                Discounts = discounts,
                UsedPromoCodeId = promoCode ==null ? null : promoCode.Id,
                UsedPromoCode = promoCode,

            };
            var token = await _tokenService.CreateToken(reservation.Id);

            _promoCodeService.ApplyPromoCode(reservation.Id, promoCode);
            var generatedPromoCode = await _promoCodeService.GeneratePromoCode(reservation.Id);

            reservation.AccessToken = token;

            reservation.GeneratedPromoCodeId = generatedPromoCode.Id;
            reservation.GeneratedPromoCode = generatedPromoCode;

            var reservationRepo = _unitOfWork.Repository<Reservation>();
            await reservationRepo.AddAsync(reservation);

            await _unitOfWork.SaveChangesAsync();

            await transaction.CommitAsync();

            var resultDto = new ReservationResultDTO
            {
                ReservationId = reservation.Id,
                Token = token.Value
            };

            return Result<ReservationResultDTO>.Success(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating reservation.");
            return Result<ReservationResultDTO>.Failure("Internal server error.", 500);
        }
    }
    
    public async Task<Result<ReservationPageViewModel>> GetReservationPage()
    {
        try
        {
            var concertRepo = _unitOfWork.Repository<Concert>();
            var concert = (await concertRepo.GetAllAsync(
                predicate: null,
                asNoTracking: true,
                includes: c => c.Dates))
                .FirstOrDefault();

            if (concert == null)
                return Result<ReservationPageViewModel>.Failure("No concert found.", 404);

            var zoneRepo = _unitOfWork.Repository<Zone>();
            var zones = (await zoneRepo.GetAllAsync()).ToList();
            var concertDateIds = concert.Dates.Select(d => d.Id).ToList();

            var tickets = await _ticketService.GetTicketsByConcertDates(concertDateIds);

            var appSettingsRepo = _unitOfWork.Repository<AppSettings>();
            var appSettings = await appSettingsRepo.GetAllAsync();

            var result = concert.ToReservationPageViewModel(zones, tickets, appSettings);

            return Result<ReservationPageViewModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching reservation page.");
            return Result<ReservationPageViewModel>.Failure("Internal server error.", 500);
        }
    }
    
    public async Task<Result<ReservationUpdateResultDTO>> UpdateReservationAsync(ReservationUpdateDTO payload)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Find existing reservation by email and access token
            var reservationRepo = _unitOfWork.Repository<Reservation>();
            var reservations = await reservationRepo.GetAllAsync(
                 r => r.Customer.Email == payload.CustomerEmail &&
                      r.AccessToken.Value == payload.AccessToken,
                 false,
                 [
                    r => r.Customer,
                    r => r.AccessToken,
                    r => r.Tickets,
                    r => r.Discounts,
                    r => r.UsedPromoCode,
                    r => r.GeneratedPromoCode
                 ]
            );

            var reservation = reservations.FirstOrDefault();
            Guid concertDateId = reservation.Tickets.FirstOrDefault()?.ConcertDateId ?? Guid.Empty;
            if (reservation == null)
                return Result<ReservationUpdateResultDTO>.Failure("Reservation not found or invalid access token.", 404);

            // Check if reservation can be modified
            if (reservation.Status == ReservationStatus.Cancelled)
                return Result<ReservationUpdateResultDTO>.Failure("Cannot modify a cancelled reservation.", 400);

            // Validate concert date exists
            var concertDateRepo = _unitOfWork.Repository<ConcertDate>();
            var concertDate = await concertDateRepo.GetByIdAsync(concertDateId);
            if (concertDate == null)
                return Result<ReservationUpdateResultDTO>.Failure("Concert date not found.", 404);

            // Get all zones
            var zones = await _zoneService.GetAllZones();
            if (zones.Count == 0)
                return Result<ReservationUpdateResultDTO>.Failure("No zones available.", 400);

            // Validate zones and capacity (considering existing tickets to be removed)
            var existingTickets = reservation.Tickets.ToList();

            // Calculate capacity considering we're replacing existing tickets
            var (isValid, error) = await _ticketService.ValidateZoneCapacitiesAsync(
                payload.Tickets, concertDateId, zones, reservation.Id);
            if (!isValid)
                return Result<ReservationUpdateResultDTO>.Failure(error ?? "Invalid ticket request.", 400);

            // Remove old tickets for this concert date
            var ticketRepo = _unitOfWork.Repository<ReservationTicket>();
            foreach (var oldTicket in existingTickets)
            {
                ticketRepo.Delete(oldTicket);
                reservation.Tickets.Remove(oldTicket);
            }

            // Remove old discounts
            var discountRepo = _unitOfWork.Repository<Discount>();
            foreach (var oldDiscount in reservation.Discounts.ToList())
            {
                discountRepo.Delete(oldDiscount);
                reservation.Discounts.Remove(oldDiscount);
            }

            // Generate new tickets and discounts
            var (newTickets, newDiscounts) = await _ticketService.GenerateTicketsAsync(
                payload.Tickets,
                concertDateId,
                zones,
                concertDate,
                reservation.UsedPromoCode);

            // Add new tickets to reservation
            foreach (var ticket in newTickets)
            {
                ticket.ReservationId = reservation.Id;
                reservation.Tickets.Add(ticket);
                await ticketRepo.AddAsync(ticket);
            }

            // Add new discounts to reservation
            foreach (var discount in newDiscounts)
            {
                discount.ReservationId = reservation.Id;
                reservation.Discounts.Add(discount);
                await discountRepo.AddAsync(discount);
            }


            // Update reservation status
            reservation.Status = ReservationStatus.Modified;

            // Update the reservation
            reservationRepo.Update(reservation);

            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync();

            // Calculate total price
            var totalPrice = reservation.Tickets.Sum(t => t.Price);
            var totalDiscount = reservation.Discounts.Sum(d => (d.Percentage / 100m) * totalPrice);
            var finalPrice = totalPrice - totalDiscount;

            var resultDto = new ReservationUpdateResultDTO
            {
                ReservationId = reservation.Id,
                Status = reservation.Status.ToString(),
                Updated = true,
                Token = reservation.AccessToken.Value,
                TotalPrice = finalPrice,
                UpdatedTicketCount = newTickets.Count(),
                Message = "Reservation updated successfully."
            };

            return Result<ReservationUpdateResultDTO>.Success(resultDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "An error occurred while updating reservation.");
            return Result<ReservationUpdateResultDTO>.Failure("Internal server error.", 500);
        }
    }
    
    public async Task<Result<ReservationDetailsViewModel>> GetReservationDetails(string accessToken, string customerEmail)
    {
        try
        {
            var reservationRepo = _unitOfWork.Repository<Reservation>();
            var reservations = await reservationRepo.GetAllAsync(
                 r => r.Customer.Email == customerEmail &&
                      r.AccessToken.Value == accessToken,
                 false,
                 [
                    r => r.Customer,
                    r => r.AccessToken,
                    r => r.Tickets,
                    r => r.Discounts,
                    r => r.UsedPromoCode,
                    r => r.GeneratedPromoCode
                 ]
            );
            var reservation = reservations.FirstOrDefault();
            if (reservation == null)
                return Result<ReservationDetailsViewModel>.Failure("Reservation not found or invalid access token.", 404);
            var concertDateId = reservation.Tickets.FirstOrDefault()?.ConcertDateId ?? Guid.Empty;
            var concertDateRepo = _unitOfWork.Repository<ConcertDate>();
            var concertDates = await concertDateRepo.GetAllAsync(predicate: c=>c.Id == concertDateId,includes: c => c.Tickets);
            var concertDate = concertDates.FirstOrDefault();
            var zoneRepo = _unitOfWork.Repository<Zone>();
            var zones = (await zoneRepo.GetAllAsync()).ToList();
            var concertDateIds = concertDate != null ? new List<Guid> { concertDate.Id } : new List<Guid>();
            var tickets = await _ticketService.GetTicketsByConcertDates(concertDateIds);
            var result = reservation.ToReservationDetailsViewModel(concertDate, zones, tickets);
            return Result<ReservationDetailsViewModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching reservation details.");
            return Result<ReservationDetailsViewModel>.Failure("Internal server error.", 500);
        }
    }
}
