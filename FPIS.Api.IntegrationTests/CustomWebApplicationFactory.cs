using FPIS.ErosRamazzottiTicketBooking.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace FPIS.Api.IntegrationTests;

/// <summary>Test fabrika koja podiže API u memoriji, zamenjuje servise mock-ovima i postavlja API ključ za testove.</summary>
public class CustomWebApplicationFactory : WebApplicationFactory<FPIS.ErosRamazzottiTicketBooking.Api.Program>
{
    /// <summary>Mock <see cref="IHomeService"/> koji se može konfigurisati u svakom testu.</summary>
    public Mock<IHomeService> HomeServiceMock { get; } = new();

    /// <summary>Mock <see cref="IReservationService"/> koji se može konfigurisati u svakom testu.</summary>
    public Mock<IReservationService> ReservationServiceMock { get; } = new();

    /// <summary>Konfiguriše test host: postavlja okruženje, in-memory konfiguraciju i menja servise mock-ovima.</summary>
    /// <param name="builder">Builder web host-a koji se konfiguriše.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ApiKey"] = "test-api-key",
                ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)\\dummy;Database=NeverUsed;Trusted_Connection=True"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IHomeService>();
            services.AddScoped(_ => HomeServiceMock.Object);

            services.RemoveAll<IReservationService>();
            services.AddScoped(_ => ReservationServiceMock.Object);
        });
    }
}
