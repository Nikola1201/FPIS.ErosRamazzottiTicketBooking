using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class ReservationTicketValidatorTests
{
    private static ReservationTicket Valid() => new()
    {
        ReservationId = Guid.NewGuid(),
        ZoneId = Guid.NewGuid(),
        ConcertDateId = Guid.NewGuid(),
        Price = 50m
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(ReservationTicketValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ReservationTicketValidator.Validate(null!));
    }

    [Fact]
    public void Validate_ReservationIdEmpty_ReturnsError()
    {
        var t = Valid(); t.ReservationId = Guid.Empty;
        Assert.Contains(ReservationTicketValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(ReservationTicket.ReservationId)));
    }

    [Fact]
    public void Validate_ZoneIdEmpty_ReturnsError()
    {
        var t = Valid(); t.ZoneId = Guid.Empty;
        Assert.Contains(ReservationTicketValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(ReservationTicket.ZoneId)));
    }

    [Fact]
    public void Validate_ConcertDateIdEmpty_ReturnsError()
    {
        var t = Valid(); t.ConcertDateId = Guid.Empty;
        Assert.Contains(ReservationTicketValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(ReservationTicket.ConcertDateId)));
    }

    [Fact]
    public void Validate_PriceNegative_ReturnsError()
    {
        var t = Valid(); t.Price = -0.01m;
        Assert.Contains(ReservationTicketValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(ReservationTicket.Price)));
    }

    [Fact]
    public void Validate_PriceZero_NoError()
    {
        var t = Valid(); t.Price = 0m;
        Assert.DoesNotContain(ReservationTicketValidator.Validate(t),
            e => e.MemberNames.Contains(nameof(ReservationTicket.Price)));
    }

    [Fact]
    public void Validate_EmptyTicket_ReturnsErrorsForAllRequiredFields()
    {
        var errors = ReservationTicketValidator.Validate(new ReservationTicket());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(ReservationTicket.ReservationId)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(ReservationTicket.ZoneId)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(ReservationTicket.ConcertDateId)));
    }
}
