using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class TokenServiceTests
{
    private static (TokenService svc, Mock<IUnitOfWork> uow, Mock<IRepository<AccessToken>> repo) Build()
    {
        var repo = new Mock<IRepository<AccessToken>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<AccessToken>()).Returns(repo.Object);
        var logger = Mock.Of<ILogger<TokenService>>();
        return (new TokenService(uow.Object, logger), uow, repo);
    }

    [Fact]
    public async Task CreateToken_Success_ReturnsTokenWithExpectedFields()
    {
        var (svc, _, repo) = Build();
        var reservationId = Guid.NewGuid();

        var token = await svc.CreateToken(reservationId);

        Assert.NotNull(token);
        Assert.NotEqual(Guid.Empty, token.Id);
        Assert.True(token.IsActive);
        Assert.Equal(reservationId, token.ReservationId);
        Assert.Equal(10, token.Value.Length);
        repo.Verify(r => r.AddAsync(It.IsAny<AccessToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateToken_WhenAddThrows_ExceptionPropagates()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.AddAsync(It.IsAny<AccessToken>())).ThrowsAsync(new InvalidOperationException("boom"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => svc.CreateToken(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateToken_ProducesDifferentTokenValuesOnSubsequentCalls()
    {
        var (svc, _, _) = Build();
        var t1 = await svc.CreateToken(Guid.NewGuid());
        var t2 = await svc.CreateToken(Guid.NewGuid());
        Assert.NotEqual(t1.Value, t2.Value);
        Assert.False(string.IsNullOrEmpty(t1.Value));
        Assert.False(string.IsNullOrEmpty(t2.Value));
    }

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TokenService(null!, Mock.Of<ILogger<TokenService>>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new TokenService(Mock.Of<IUnitOfWork>(), null!));
    }
}
