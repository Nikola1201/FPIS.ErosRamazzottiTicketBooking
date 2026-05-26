using FPIS.ErosRamazzottiTicketBooking.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FPIS.Api.Tests.Middleware;

public class ApiKeyMiddlewareTests
{
    private static IConfiguration BuildConfig(string? apiKey)
    {
        var inMemory = new Dictionary<string, string?> { ["ApiKey"] = apiKey };
        return new ConfigurationBuilder().AddInMemoryCollection(inMemory!).Build();
    }

    [Fact]
    public async Task ValidApiKey_InvokesNext()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Request.Headers["X-Api-Key"] = "secret";
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.True(nextCalled);
        Assert.Equal(200, ctx.Response.StatusCode); // Default
    }

    [Fact]
    public async Task MissingApiKey_Returns401()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.False(nextCalled);
        Assert.Equal(401, ctx.Response.StatusCode);
    }

    [Fact]
    public async Task MismatchedApiKey_Returns401()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.Request.Headers["X-Api-Key"] = "wrong";
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.False(nextCalled);
        Assert.Equal(401, ctx.Response.StatusCode);
    }
}
