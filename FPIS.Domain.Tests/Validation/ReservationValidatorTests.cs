using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class ReservationValidatorTests
{
    private static Reservation Valid() => new()
    {
        Customer = new Customer(),
        AccessToken = new AccessToken(),
        Status = ReservationStatus.Active,
        Tickets = new List<ReservationTicket> { new ReservationTicket() },
        Discounts = new List<Discount>()
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(ReservationValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ReservationValidator.Validate(null!));
    }

    [Fact]
    public void Validate_CustomerNull_ReturnsError()
    {
        var r = Valid(); r.Customer = null!;
        Assert.Contains(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.Customer)));
    }

    [Fact]
    public void Validate_AccessTokenNull_ReturnsError()
    {
        var r = Valid(); r.AccessToken = null!;
        Assert.Contains(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.AccessToken)));
    }

    [Fact]
    public void Validate_StatusUndefined_ReturnsError()
    {
        var r = Valid(); r.Status = (ReservationStatus)999;
        Assert.Contains(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.Status)));
    }

    [Fact]
    public void Validate_TicketsNull_ReturnsError()
    {
        var r = Valid(); r.Tickets = null!;
        Assert.Contains(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.Tickets)));
    }

    [Fact]
    public void Validate_TicketsEmpty_ReturnsError()
    {
        var r = Valid(); r.Tickets = new List<ReservationTicket>();
        Assert.Contains(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.Tickets)));
    }

    [Fact]
    public void Validate_DiscountsNull_ReturnsError()
    {
        var r = Valid(); r.Discounts = null!;
        Assert.Contains(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.Discounts)));
    }

    [Fact]
    public void Validate_DiscountsEmpty_NoError()
    {
        var r = Valid(); r.Discounts = new List<Discount>();
        Assert.DoesNotContain(ReservationValidator.Validate(r),
            e => e.MemberNames.Contains(nameof(Reservation.Discounts)));
    }

    [Fact]
    public void Validate_EmptyReservation_ReturnsErrorsForRequiredFields()
    {
        var errors = ReservationValidator.Validate(new Reservation());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Reservation.Customer)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Reservation.AccessToken)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(Reservation.Tickets)));
    }
}
