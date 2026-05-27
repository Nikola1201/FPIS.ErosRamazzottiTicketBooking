using Microsoft.AspNetCore.Http;
namespace FPIS.ErosRamazzottiTicketBooking.Api.Middleware;

/// <summary>
/// Middleware koji zahteva ispravan API ključ u <c>X-Api-Key</c> header-u; inače vraća 401.
/// </summary>
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-Api-Key";

    /// <summary>Konstruktor sa sledećim middleware-om u pipeline-u.</summary>
    /// <param name="next">Sledeći delegat u pipeline-u.</param>
    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>Validira API ključ i prosleđuje zahtev dalje ili vraća 401.</summary>
    /// <param name="context">HTTP kontekst.</param>
    /// <param name="configuration">Konfiguracija sa očekivanim API ključem (key <c>ApiKey</c>).</param>
    /// <returns>Zadatak koji se završava kada je zahtev obrađen.</returns>
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var apiKey = configuration.GetValue<string>("ApiKey");

        if (!string.Equals(apiKey, extractedApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}
