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

    private static async Task<string> ReadBodyAsync(HttpContext ctx)
    {
        ctx.Response.Body.Position = 0;
        using var reader = new StreamReader(ctx.Response.Body);
        return await reader.ReadToEndAsync();
    }

    [Fact]
    public async Task ValidApiKey_InvokesNext()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.Request.Headers["X-Api-Key"] = "secret";
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.True(nextCalled);
        Assert.Equal(string.Empty, await ReadBodyAsync(ctx));
    }

    [Fact]
    public async Task MissingApiKey_Returns401WithExpectedBody()
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
        Assert.Equal("API Key was not provided.", await ReadBodyAsync(ctx));
    }

    [Fact]
    public async Task MismatchedApiKey_Returns401WithExpectedBody()
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
        Assert.Equal("Unauthorized client.", await ReadBodyAsync(ctx));
    }

    [Fact]
    public async Task EmptyApiKeyHeader_Returns401()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.Request.Headers["X-Api-Key"] = "";
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.False(nextCalled);
        Assert.Equal(401, ctx.Response.StatusCode);
        Assert.Equal("Unauthorized client.", await ReadBodyAsync(ctx));
    }

    [Fact]
    public async Task NullConfiguredApiKey_Returns401WithoutThrowing()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.Request.Headers["X-Api-Key"] = "anything";
        var config = BuildConfig(null);

        await middleware.InvokeAsync(ctx, config);

        Assert.False(nextCalled);
        Assert.Equal(401, ctx.Response.StatusCode);
    }

    [Fact]
    public async Task HeaderNameIsCaseInsensitive()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.Request.Headers["x-api-key"] = "secret";
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.True(nextCalled);
    }

    [Fact]
    public async Task ApiKeyComparisonIsCaseSensitive()
    {
        var nextCalled = false;
        RequestDelegate next = ctx => { nextCalled = true; return Task.CompletedTask; };
        var middleware = new ApiKeyMiddleware(next);

        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        ctx.Request.Headers["X-Api-Key"] = "SECRET";
        var config = BuildConfig("secret");

        await middleware.InvokeAsync(ctx, config);

        Assert.False(nextCalled);
        Assert.Equal(401, ctx.Response.StatusCode);
    }
}
