using System.Linq.Expressions;
using FPIS.Domain.Models;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using FPIS.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FPIS.Api.Tests.Services;

public class AppSettingsServiceTests
{
    private static (AppSettingsService svc, Mock<IUnitOfWork> uow, Mock<IRepository<AppSettings>> repo) Build()
    {
        var repo = new Mock<IRepository<AppSettings>>();
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Repository<AppSettings>()).Returns(repo.Object);
        var logger = Mock.Of<ILogger<AppSettingsService>>();
        return (new AppSettingsService(uow.Object, logger), uow, repo);
    }

    [Fact]
    public async Task GetAppSettingsAsync_ReturnsDictionaryFromRepository()
    {
        var (svc, _, repo) = Build();
        var data = new List<AppSettings>
        {
            new() { Key = "MaxTickets", Value = "10" },
            new() { Key = "PromoActive", Value = "true" }
        };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<AppSettings, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<AppSettings, object>>[]>()))
            .ReturnsAsync(data);

        var result = await svc.GetAppSettingsAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("10", result["MaxTickets"]);
        Assert.Equal("true", result["PromoActive"]);
    }

    [Fact]
    public async Task GetAppSettingsAsync_WhenRepoThrows_ReturnsEmptyDictionary()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<AppSettings, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<AppSettings, object>>[]>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var result = await svc.GetAppSettingsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAppSettingsAsync_EmptyRepo_ReturnsEmptyDictionary()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<AppSettings, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<AppSettings, object>>[]>()))
            .ReturnsAsync(new List<AppSettings>());

        var result = await svc.GetAppSettingsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDiscountSettings_ParsesAllFourValues()
    {
        var (svc, _, repo) = Build();
        var data = new List<AppSettings>
        {
            new() { Key = "EarlyBirdDiscountPercentage", Value = "15" },
            new() { Key = "EarlyBirdDiscountDaysBefore", Value = "30" },
            new() { Key = "FifthTicketDiscountPercentage", Value = "20" },
            new() { Key = "FriendPromoDiscountPercentage", Value = "10" }
        };
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<AppSettings, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<AppSettings, object>>[]>()))
            .ReturnsAsync(data);

        var (earlyBird, days, fifth, friend) = await svc.GetDiscountSettings();

        Assert.Equal(15, earlyBird);
        Assert.Equal(30, days);
        Assert.Equal(20, fifth);
        Assert.Equal(10, friend);
    }

    [Fact]
    public async Task GetDiscountSettings_MissingKeys_DefaultsToZero()
    {
        var (svc, _, repo) = Build();
        repo.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<AppSettings, bool>>?>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<AppSettings, object>>[]>()))
            .ReturnsAsync(new List<AppSettings>());

        var (earlyBird, days, fifth, friend) = await svc.GetDiscountSettings();

        Assert.Equal(0, earlyBird);
        Assert.Equal(0, days);
        Assert.Equal(0, fifth);
        Assert.Equal(0, friend);
    }

    [Fact]
    public void Constructor_NullUnitOfWork_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new AppSettingsService(null!, Mock.Of<ILogger<AppSettingsService>>()));
    }

    [Fact]
    public void Constructor_NullLogger_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new AppSettingsService(Mock.Of<IUnitOfWork>(), null!));
    }
}
