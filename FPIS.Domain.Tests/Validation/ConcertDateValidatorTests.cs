using FPIS.Domain.Models;
using FPIS.Domain.Validation;
using Xunit;

namespace FPIS.Domain.Tests.Validation;

public class ConcertDateValidatorTests
{
    private static ConcertDate Valid() => new()
    {
        Date = new DateTime(2026, 6, 15, 20, 30, 0, DateTimeKind.Utc),
        ConcertId = Guid.NewGuid()
    };

    [Fact]
    public void Validate_Valid_ReturnsEmpty()
    {
        Assert.Empty(ConcertDateValidator.Validate(Valid()));
    }

    [Fact]
    public void Validate_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ConcertDateValidator.Validate(null!));
    }

    [Fact]
    public void Validate_DateDefault_ReturnsError()
    {
        var cd = Valid(); cd.Date = default;
        Assert.Contains(ConcertDateValidator.Validate(cd),
            e => e.MemberNames.Contains(nameof(ConcertDate.Date)));
    }

    [Fact]
    public void Validate_ConcertIdEmpty_ReturnsError()
    {
        var cd = Valid(); cd.ConcertId = Guid.Empty;
        Assert.Contains(ConcertDateValidator.Validate(cd),
            e => e.MemberNames.Contains(nameof(ConcertDate.ConcertId)));
    }

    [Fact]
    public void Validate_EmptyConcertDate_ReturnsErrorsForAllRequiredFields()
    {
        var errors = ConcertDateValidator.Validate(new ConcertDate());
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(ConcertDate.Date)));
        Assert.Contains(errors, e => e.MemberNames.Contains(nameof(ConcertDate.ConcertId)));
    }
}
