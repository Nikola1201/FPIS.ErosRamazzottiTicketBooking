using System.Net;
using System.Text.Json;
using FPIS.Domain.ViewModels;
using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Moq;

namespace FPIS.Api.IntegrationTests.Controllers;

/// <summary>HTTP integracioni testovi za <c>GET /api/home</c> — verifikuje status kodove i JSON oblik odgovora.</summary>
public class HomeControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    /// <summary>Konstruktor; čuva referencu na deljenu test fabriku i resetuje mock pre svakog testa.</summary>
    /// <param name="factory">Deljena test fabrika preko <see cref="IClassFixture{T}"/>.</param>
    public HomeControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _factory.HomeServiceMock.Reset();
    }

    /// <summary>GET /api/home — uspeh vraća 200, application/json i JSON sa svim očekivanim ključevima.</summary>
    [Fact]
    public async Task Get_Home_Success_Returns200_WithExpectedJsonShape()
    {
        var vm = new HomePageViewModel
        {
            Title = "T",
            Subtitle = "S",
            ImageUrl = "https://x",
            Description = "D",
            ButtonText = "B",
            ButtonUrl = "/r",
            Concert = new ConcertViewModel
            {
                Title = "C",
                City = "Beograd",
                Venue = "V",
                Address = "A",
                Dates = new List<DateTime> { DateTime.UtcNow },
                AdditionalInfo = ""
            }
        };
        _factory.HomeServiceMock
            .Setup(s => s.GetHomePage())
            .ReturnsAsync(Result<HomePageViewModel>.Success(vm));

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "test-api-key");

        var response = await client.GetAsync("/api/home");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        AssertProperty(root, "title", JsonValueKind.String);
        AssertProperty(root, "subtitle", JsonValueKind.String);
        AssertProperty(root, "imageUrl", JsonValueKind.String);
        AssertProperty(root, "description", JsonValueKind.String);
        AssertProperty(root, "buttonText", JsonValueKind.String);
        AssertProperty(root, "buttonUrl", JsonValueKind.String);

        Assert.True(root.TryGetProperty("concert", out var concert), "Missing 'concert' property.");
        Assert.Equal(JsonValueKind.Object, concert.ValueKind);
        AssertProperty(concert, "title", JsonValueKind.String);
        AssertProperty(concert, "city", JsonValueKind.String);
        AssertProperty(concert, "venue", JsonValueKind.String);
        AssertProperty(concert, "address", JsonValueKind.String);
        Assert.True(concert.TryGetProperty("dates", out var dates), "Missing 'concert.dates' property.");
        Assert.Equal(JsonValueKind.Array, dates.ValueKind);
        AssertProperty(concert, "additionalInfo", JsonValueKind.String);
    }

    /// <summary>GET /api/home — servis vraća 404 i odgovor je <see cref="Microsoft.AspNetCore.Mvc.ProblemDetails"/> sa očekivanim ključevima.</summary>
    [Fact]
    public async Task Get_Home_ServiceReturns404_Returns404_WithProblemDetailsShape()
    {
        _factory.HomeServiceMock
            .Setup(s => s.GetHomePage())
            .ReturnsAsync(Result<HomePageViewModel>.Failure("No concert found.", 404));

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", "test-api-key");

        var response = await client.GetAsync("/api/home");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        AssertProperty(root, "title", JsonValueKind.String);
        Assert.True(root.TryGetProperty("status", out var status), "Missing 'status' property.");
        Assert.Equal(JsonValueKind.Number, status.ValueKind);
        Assert.Equal(404, status.GetInt32());
        AssertProperty(root, "detail", JsonValueKind.String);
    }

    /// <summary>GET /api/home — bez <c>X-Api-Key</c> header-a, middleware vraća 401.</summary>
    [Fact]
    public async Task Get_Home_MissingApiKey_Returns401()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/home");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static void AssertProperty(JsonElement element, string name, JsonValueKind expectedKind)
    {
        Assert.True(element.TryGetProperty(name, out var prop), $"Missing '{name}' property.");
        Assert.Equal(expectedKind, prop.ValueKind);
    }
}
